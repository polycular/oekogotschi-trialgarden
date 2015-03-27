using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ToolbAR.Vuforia
{
    public class ARDataSetBehaviour : MonoBehaviour
    {
        protected DataSet mDataSet = null;
        [SerializeField]
        private string mName = "";
        private bool mInitialized = false;

        private List<ARTrackableBehaviour> mCurrentlyTracked = new List<ARTrackableBehaviour>();
        private List<ARTrackableBehaviour> mCurrentlyHardTracked = new List<ARTrackableBehaviour>();
        private List<ARTrackableBehaviour> mCurrentlySoftTracked = new List<ARTrackableBehaviour>();

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }
        public bool hasName
        {
            get
            {
                return (mName != "");
            }
        }
        //Null if not loaded
        public DataSet DataSet
        {
            get
            {
                return mDataSet;
            }
        }

        public List<ARTrackableBehaviour> CurrentlyTracked
        {
            get
            {
                return mCurrentlyTracked;
            }
        }
        public List<ARTrackableBehaviour> CurrentlyHardTracked
        {
            get
            {
                return mCurrentlyHardTracked;
            }
        }
        public List<ARTrackableBehaviour> CurrentlySoftTracked
        {
            get
            {
                return mCurrentlySoftTracked;
            }
        }

        void loadDataSet(ImageTracker QCARImageTracker)
        {
            if (this.mDataSet != null)
            {
                LogAR.logError("Cannot load data set " + mDataSet.Path + " twice.", gameObject);
            }

            //Set active shortly to allow vuforia to find the trackables.. hrmpf -.-
            bool realActiveState = gameObject.activeSelf;
            gameObject.SetActive(true);
            activateTrackables();
            mDataSet = ARUtilities.DataSetManager.loadOrGetDataSet(mName, QCARImageTracker).DataSet;
            gameObject.SetActive(realActiveState);

            if (mDataSet == null)
            {
                LogAR.logError("Failed to load data set " + mName + ".", gameObject);
                gameObject.SetActive(false);
            }
        }


        void onQCARImageTrackerInit(ImageTracker QCARImageTracker)
        {
            ARScene.Instance.onQCARImageTrackerInit -= this.onQCARImageTrackerInit;
            initialize();
            gameObject.SetActive(true);
        }

        void activateTrackables()
        {
            ARTrackableBehaviour[] trackables = GetComponentsInChildren<ARTrackableBehaviour>(true);
            foreach (ARTrackableBehaviour trackable in trackables)
            {
                trackable.gameObject.SetActive(true);
            }
        }
        void deactivateTrackables()
        {
            ARTrackableBehaviour[] trackables = GetComponentsInChildren<ARTrackableBehaviour>(true);
            foreach (ARTrackableBehaviour trackable in trackables)
            {
                trackable.gameObject.SetActive(false);
            }
        }

        void initialize()
        {
            if (mInitialized)
                return;

            this.loadDataSet(ARScene.Instance.QCARImageTracker);
            mInitialized = true;
        }

        #region Unity Messages

        void Update()
        {
            mCurrentlyTracked.Clear();
            mCurrentlySoftTracked.Clear();
            mCurrentlyHardTracked.Clear();
            ARTrackableBehaviour[] trackables = GetComponentsInChildren<ARTrackableBehaviour>(true);
            foreach (ARTrackableBehaviour trackable in trackables)
            {
                if (trackable.IsSoftTracked)
                {
                    mCurrentlySoftTracked.Add(trackable);
                }
                if (trackable.IsHardTracked)
                {
                    mCurrentlyHardTracked.Add(trackable);
                }
                if (trackable.IsTracked)
                {
                    mCurrentlyTracked.Add(trackable);
                }
            }
        }

        void OnEnable()
        {
            if (ARScene.Instance.QCARImageTracker == null)
            {
                gameObject.SetActive(false);
                ARScene.Instance.onQCARImageTrackerInit -= this.onQCARImageTrackerInit;
                ARScene.Instance.onQCARImageTrackerInit += this.onQCARImageTrackerInit;
            }
            else
            {
                initialize();
                activateTrackables();
                if (!ARScene.Instance.QCARImageTracker.ActivateDataSet(mDataSet))
                {
                    gameObject.SetActive(false);
                }
            }

        }
        void OnDisable()
        {
            if (mInitialized)
            {
                if (!ARScene.Instance.QCARImageTracker.DeactivateDataSet(mDataSet))
                {
                    gameObject.SetActive(true);
                }
            }

        }

        void OnDestroy()
        {
            ARScene.Instance.onQCARImageTrackerInit -= this.onQCARImageTrackerInit;
            if (mDataSet != null)
            {
                StateManager stateManager = TrackerManager.Instance.GetStateManager();
                foreach (Trackable trackable in mDataSet.GetTrackables())
                {
                    stateManager.DestroyTrackableBehavioursForTrackable(trackable, false);
                }
                //Cannot destroy datasets from disk
                //ARScene.Instance.QCARImageTracker.DestroyDataSet(mDataSet, false);
                ARUtilities.DataSetManager.unloadDataSet(this.mName, ARScene.Instance.QCARImageTracker);
            }
        }

        #endregion
    }
}