using UnityEngine;
using System.Collections;
using System.Linq;

namespace ToolbAR.Vuforia
{
/// <summary>
/// This eases the pain of camera management across scenes.
/// Only one ARSceneCamera can exist at any given time, but they will replace each other when scenes are changed
/// </summary>
    [RequireComponent(typeof(Camera))]
    public class ARSceneCameraBehaviour : MonoBehaviour
    {

        Camera mCamera = null;
        ARCameraBehaviour mARCamera = null;
        int mCachedCullingMask = 0;

        [Tooltip("If set, the GlobalARCamera will jump to the position & rotation of this Scene Camera once the scene is entered")]
        public bool ImprintOnGlobalCamera = false;

        [Tooltip("If set, the GlobalARCamera will use the CullingMask of this Camera affecting the Local Layers (10-19)")]
        public bool AutoApplyLocalCullingMask = true;

        public Camera Camera
        {
            get
            {
                return mCamera;
            }
        }

        public ARCameraBehaviour ARCamera
        {
            get
            {
                if (mARCamera == null)
                    mARCamera = ARScene.Behaviour.ARCameraBehaviour;
                return mARCamera;
            }
        }


        public QCARBehaviour QCARBehaviour
        {
            get
            {
                return ARCamera.QCARBehaviour;
            }
        }

        public GameObject TextureBufferCamera
        {
            get
            {
                return ARCamera.TextureBufferCamera;
            }
        }
        public GameObject BackgroundCamera
        {
            get
            {
                return ARCamera.BackgroundCamera;
            }
        }

        public void applyLocalCullingMask(Camera target)
        {
            //Get all sets bit in the local mask range of this camera
            int localMask = Camera.cullingMask & BITMASK_LOCAL_LAYERS;
            //Get a bitmask of the target without the local layers set (effectively disabling all local layers)
            int cleanedGlobalMask = target.cullingMask & BITMASK_GLOBAL_LAYERS;
            //Apply the combinative mask
            int mergedMask = localMask | cleanedGlobalMask;
            target.cullingMask = mergedMask;
        }

        #region Unity Events

        void Awake()
        {
            mCamera = this.GetComponent<Camera>();
            mCamera.enabled = false;
            mARCamera = ARScene.Behaviour.ARCameraBehaviour;

            if (ImprintOnGlobalCamera)
            {
                ARCamera.transform.position = transform.position;
                ARCamera.transform.rotation = transform.rotation;
            }

            transform.position = ARCamera.transform.position;
            transform.rotation = ARCamera.transform.rotation;


            if (AutoApplyLocalCullingMask)
            {
                applyLocalCullingMask(ARCamera.Camera);
            }
            mCachedCullingMask = Camera.cullingMask;
        }

        /// <summary>
        /// Bitmask for all local layers 10-19 (L0-L9)
        /// </summary>
        public const int BITMASK_LOCAL_LAYERS = 1047552;

        /// <summary>
        /// Bitmask for all non-local layers
        /// </summary>
        public const int BITMASK_GLOBAL_LAYERS = ~BITMASK_LOCAL_LAYERS;

        void Update()
        {
            transform.position = ARCamera.transform.position;
            transform.rotation = ARCamera.transform.rotation;


            Camera globalcam = ARCamera.Camera;

            mCamera.nearClipPlane = globalcam.nearClipPlane;
            mCamera.farClipPlane = globalcam.farClipPlane;
            mCamera.fieldOfView = globalcam.fieldOfView;
            mCamera.depthTextureMode = globalcam.depthTextureMode;
            mCamera.eventMask = globalcam.eventMask;
            mCamera.hdr = globalcam.hdr;
            mCamera.layerCullDistances = globalcam.layerCullDistances;
            mCamera.layerCullSpherical = globalcam.layerCullSpherical;
            mCamera.orthographic = globalcam.orthographic;
            mCamera.orthographicSize = globalcam.orthographicSize;
            mCamera.pixelRect = globalcam.pixelRect;
            mCamera.rect = globalcam.rect;
            mCamera.renderingPath = globalcam.renderingPath;
            mCamera.targetTexture = globalcam.targetTexture;
            mCamera.transparencySortMode = globalcam.transparencySortMode;
            mCamera.useOcclusionCulling = globalcam.useOcclusionCulling;
            //Could be excluded because of Vuforia overriding/needing them
            mCamera.backgroundColor = globalcam.backgroundColor;
            mCamera.clearFlags = globalcam.clearFlags;
            mCamera.cullingMask = globalcam.cullingMask;
            mCamera.projectionMatrix = globalcam.projectionMatrix;
            mCamera.worldToCameraMatrix = globalcam.worldToCameraMatrix;
            

            if (mCachedCullingMask != Camera.cullingMask)
            {
                mCachedCullingMask = Camera.cullingMask;
                if (AutoApplyLocalCullingMask)
                {
                    applyLocalCullingMask(globalcam);
                }
            }

            
        }

        #endregion
    }
}