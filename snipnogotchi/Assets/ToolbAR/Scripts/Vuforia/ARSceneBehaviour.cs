using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ToolbAR.Vuforia
{
    /// <summary>
    /// This is the basic management clas for our managed AR Scene setup.
    /// This behaviour is needed in any scene that wants to work with ToolbAR.Vuforia
    /// </summary>
    public class ARSceneBehaviour : MonoBehaviour
    {

        public ARCameraBehaviour ARCameraPrefab;

        public List<ARDataSetBehaviour> getDataSets()
        {
            List<ARDataSetBehaviour> list = new List<ARDataSetBehaviour>();

            for (int i = 0; i < transform.childCount; i++)
            {
                ARDataSetBehaviour dsb = transform.GetChild(i).GetComponent<ARDataSetBehaviour>();
                if (dsb != null)
                {
                    list.Add(dsb);
                }
            }

            return list;
        }

        void clearDataSets()
        {
            foreach (ARDataSetBehaviour dataset in getDataSets())
            {
                DestroyImmediate(dataset.gameObject);
            }
        }

        public ARCameraBehaviour ARCameraBehaviour
        {
            get
            {
                return mARCamera;
            }
        }

        private bool mInitialized = false;
        private ARCameraBehaviour mARCamera = null;

        ARSceneBehaviour takeOverScene(ARSceneBehaviour previous)
        {
            //Ensure that the previous was initialized
            previous.initialize();


            //takeover the old ARCamera
            mARCamera = previous.mARCamera;
            mARCamera.transform.parent = this.transform;

            //Disable the old scene so nothing of it will get in our way
            previous.gameObject.SetActive(false);
            previous.clearDataSets();

            Destroy(previous.gameObject);
            //Set this to initialized, otheriwse the next takeover would start an initialization on this
            mInitialized = true;
            return this;
        }

        void initialize()
        {
            if (mInitialized)
                return;

            if (ARCameraPrefab == null)
                throw new System.Exception("ARCameraPrefab has to be set in ARScene");



            mARCamera = (Instantiate(ARCameraPrefab.gameObject) as GameObject).GetComponent<ARCameraBehaviour>();
            mARCamera.name = "ARCamera(Global)";
            mARCamera.transform.parent = this.transform;
            mInitialized = true;
            ARScene.relocateBehaviour(this);
        }

        #region Unity Messages

        void Awake()
        {
            ARSceneBehaviour globalBehaviour = ARScene.Behaviour;
            if (globalBehaviour == this || globalBehaviour == null)
            {
                initialize();
            }
            else
            {
                ARScene.relocateBehaviour(takeOverScene(ARScene.Behaviour));
            }

            DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {
            ARScene.Instance.update();
        }

        void OnEnable()
        {
        }

        void OnDisable()
        {
        }

        #endregion
    }
}