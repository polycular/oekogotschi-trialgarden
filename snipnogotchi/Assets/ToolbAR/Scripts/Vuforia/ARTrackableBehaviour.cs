using UnityEngine;
using System.Collections;

namespace ToolbAR.Vuforia
{
    public class ARTrackableBehaviour : MonoBehaviour, ITrackableEventHandler
    {
        #region Events

        public delegate void TrackableStateChangeHandler(ARTrackableBehaviour agent);
        public event TrackableStateChangeHandler onTrackableStateChange;

        #endregion

        #region Public Properties


        public bool IsRenderingTarget = false;


        public Trackable Trackable
        {
            get { return mWatchedTrackable.Trackable; }
        }
        public bool HasTrackable
        {
            get { return HasTrackableBehaviour && Trackable != null; }
        }
        public TrackableBehaviour TrackableBehaviour
        {
            get { return mWatchedTrackable; }
        }
        public bool HasTrackableBehaviour
        {
            get { return mWatchedTrackable != null; }
        }

        public int ID
        {
            get
            {
                return mID;
            }
        }
        public string Name
        {
            get
            {
                return mName;
            }
        }

        public ARDataSetBehaviour ARDataSet
        {
            get
            {
                if (transform.parent == null)
                    return null;
                else
                    return transform.parent.GetComponent<ARDataSetBehaviour>();
            }
        }

        public TrackableBehaviour.Status Status
        {
            get
            {
                return mStatus;
            }
        }
        public TrackableBehaviour.Status PreviousStatus
        {
            get
            {
                return mPreviousStatus;
            }
        }
        //Returns if the Trackable is expelceitley (inlcuding extended) Tracked only
        public bool IsHardTracked
        {
            get
            {
                return (
                    Status == global::TrackableBehaviour.Status.DETECTED
                    ||
                    Status == global::TrackableBehaviour.Status.TRACKED
                    ||
                    Status == global::TrackableBehaviour.Status.EXTENDED_TRACKED
                    );
            }
        }
        //Returns true if the target is soft tracked (only lost for a short time) only, (false if it is hard tracked)
        public bool IsSoftTracked
        {
            get
            {
                return false; //@todo implement soft tracking
            }
        }
        //Returns true if the target is soft or hard tracked
        public bool IsTracked
        {
            get
            {
                return (IsHardTracked || IsSoftTracked);
            }
        }

        #endregion

        #region Internal Properties

        TrackableBehaviour mWatchedTrackable = null;
        int mID = 0;
        string mName = "Unknown";
        TrackableBehaviour.Status mStatus = TrackableBehaviour.Status.UNKNOWN;
        TrackableBehaviour.Status mPreviousStatus = TrackableBehaviour.Status.UNKNOWN;

        #endregion

        #region Internal Methods

        protected TrackableBehaviour determineTrackableBehaviour()
        {
            return this.GetComponent<TrackableBehaviour>();
        }

        #endregion

        #region Unity Events

        void Awake()
        {
            if (ARDataSet == null)
            {
                LogAR.logWarning("ARTrackableBehaviour outside ARDataSet deactivated", this, this);
                this.gameObject.SetActive(false);
            }

            mWatchedTrackable = determineTrackableBehaviour();
            if (mWatchedTrackable == null)
            {
                LogAR.logWarning("ARTrackableBehaviour without any form of TrackableBehaviour (e.g. ImageTarget) deactivated", this, this);
                this.gameObject.SetActive(false);
            }

            if (QCARRuntimeUtilities.IsQCAREnabled())
            {
                // We disable the mesh components at run-time only, but keep them for
                // visualization when running in the editor:
                if (this.renderer)
                    this.renderer.enabled = IsRenderingTarget;
            }
        }

        void OnEnable()
        {
            mWatchedTrackable.RegisterTrackableEventHandler(this);
        }

        void OnDisable()
        {
            if (mWatchedTrackable != null)
            {
                mWatchedTrackable.UnregisterTrackableEventHandler(this);
            }
        }

        #endregion

        #region Vuforia ITrackableEventHandler

        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {
            this.mPreviousStatus = previousStatus;
            this.mStatus = newStatus;

            if (HasTrackable)
            {
                this.mID = Trackable.ID;
                this.mName = Trackable.Name;
            }


            if (onTrackableStateChange != null)
                onTrackableStateChange(this);
        }

        #endregion
    }

}