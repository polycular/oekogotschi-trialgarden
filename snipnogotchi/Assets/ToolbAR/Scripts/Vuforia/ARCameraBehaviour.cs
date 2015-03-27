using UnityEngine;
using System.Collections;

namespace ToolbAR.Vuforia
{
    /// <summary>
    /// Soft wrapper fr the QCAR Behaviour
    /// Manages ARCamera and alows easy adding of features.
    /// There can only be one ARCamera, and it will be spawned at runtime.
    /// To attach something to the camera like you're used to, please provide
    /// an ARSceneCamera (which is always appended to the ARCamera)
    /// </summary>
    [RequireComponent(typeof(QCARBehaviour))]
    public class ARCameraBehaviour : MonoBehaviour
    {
        public Camera Camera
        {
            get
            {
                return mCamera;
            }
        }
        public QCARBehaviour QCARBehaviour
        {
            get
            {
                return mQCARBehaviour;
            }
        }

        public GameObject TextureBufferCamera
        {
            get
            {
                return mTextureBufferCamera;
            }
        }
        public GameObject BackgroundCamera
        {
            get
            {
                return mBackgroundCamera;
            }
        }
        public bool IsUsingCameraDevice
        {
            get
            {
                return ARScene.Instance.IsUsingCameraDevice;
            }
            set
            {
                ARScene.Instance.IsUsingCameraDevice = value;
            }
        }

        #region Explicit Casts

        public static explicit operator QCARBehaviour(ARCameraBehaviour b)
        {
            return b.QCARBehaviour;
        }
        public static explicit operator Camera(ARCameraBehaviour b)
        {
            return b.Camera;
        }

        #endregion

        Camera mCamera = null;
        QCARBehaviour mQCARBehaviour = null;
        GameObject mTextureBufferCamera = null;
        GameObject mBackgroundCamera = null;


        #region Unity Events

        void Awake()
        {
            mCamera = this.GetComponent<Camera>();
        }

        void LateUpdate()
        {
            //Find unitys generatedcmaeras
            if (mTextureBufferCamera == null)
            {
                mTextureBufferCamera = GameObject.Find("TextureBufferCamera");
                if (mTextureBufferCamera != null)
                    mTextureBufferCamera.transform.parent = this.transform;
            }
            if (mBackgroundCamera == null)
            {
                SetBGCameraLayerBehaviour bgcam = GameObject.FindObjectOfType<SetBGCameraLayerBehaviour>() as SetBGCameraLayerBehaviour;
                if (bgcam != null)
                {
                    mBackgroundCamera = bgcam.gameObject;
                    mBackgroundCamera.transform.parent = this.transform;
                }
            }
        }

        #endregion
    }
}