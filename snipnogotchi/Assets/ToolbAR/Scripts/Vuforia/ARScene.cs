using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ToolbAR.Vuforia
{
    public class ARScene
    {

        static public ARScene Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new ARScene();
                }
                return mInstance;
            }
        }

        static public ARSceneBehaviour Behaviour
        {
            get
            {
                if (mGameObject == null)
                    relocateBehaviour();
                return mGameObject;
            }
        }

        static public ARSceneBehaviour GameObject
        {
            get
            {
                return Behaviour;
            }
        }


        static public bool HasInstance
        {
            get
            {
                return (mInstance != null);
            }
        }

        static public void relocateBehaviour()
        {
            relocateBehaviour(UnityEngine.GameObject.FindObjectOfType<ARSceneBehaviour>());
        }
        static public void relocateBehaviour(ARSceneBehaviour newSceneObject)
        {
            mGameObject = newSceneObject;
        }

#if UNITY_EDITOR
        static public List<string> getFreeDataSets()
        {
            List<string> free = ARUtilities.getStoredDataSets();
            List<ARDataSetBehaviour> occupied =
                new List<ARDataSetBehaviour>(UnityEngine.GameObject.FindObjectsOfType<ARDataSetBehaviour>());

            foreach (ARDataSetBehaviour behav in occupied)
            {
                if (behav.Name != null)
                    free.Remove(behav.Name);
            }
            return free;
        }
#endif


        public bool IsUsingCameraDevice
        {
            get
            {
                return mIsUsingCameraDevice;
            }
            set
            {
                mIsUsingCameraDeviceTarget = value;
                updateCameraDevice();
            }
        }

        public ImageTracker QCARImageTracker
        {
            get
            {
                return mQCARImageTracker;
            }
        }

        //Updates states, checkups with vuforia etc.
        public void update()
        {
            bool prevState = (mQCARImageTracker != null);
            mQCARImageTracker = (ImageTracker)TrackerManager.Instance.GetTracker<ImageTracker>();
            if (prevState == false && mQCARImageTracker != null)
            {
                if (onQCARImageTrackerInit != null)
                {
                    onQCARImageTrackerInit(mQCARImageTracker);
                }
            }
            updateCameraDevice();
        }

        public delegate void OnQCARImageTrackerInitHandler(ImageTracker QCARImageTracker);
        public event OnQCARImageTrackerInitHandler onQCARImageTrackerInit;

        static ARScene mInstance = null;
        static ARSceneBehaviour mGameObject = null;
        ImageTracker mQCARImageTracker = null;
        bool mIsUsingCameraDeviceTarget = true;
        bool mIsUsingCameraDevice = true;

        private ARScene()
        {
        }

        private void updateCameraDevice()
        {
            if (mQCARImageTracker != null && mIsUsingCameraDevice != mIsUsingCameraDeviceTarget)
            {
                if (mIsUsingCameraDeviceTarget && CameraDevice.Instance.Start())
                {
                    mIsUsingCameraDevice = true;
                }
                else if (!mIsUsingCameraDeviceTarget && CameraDevice.Instance.Stop())
                {
                    mIsUsingCameraDevice = false;
                }
            }
        }
    }
}