using UnityEngine;

namespace ToolbAR.Sensors
{
    /// <summary>
    /// This class wraps the acceleration sensor. It triggers a shake event if a shake with a minimum shake length of MinShakeLengthInSeconds
    /// and an average shake strength of minimum ShakeStrengthThreshold was recognized. 
    /// </summary>
    public class ShakeRecognitionBehaviour : MonoBehaviour
    {
        public float UpdateIntervalInSeconds = 0f;
        public float PauseBetweenShakesInSeconds = 0.2f;
        public float ShakeStrengthThreshold = 0.7f;
        public float MinShakeLengthInSeconds = 0.2f;

        public delegate void OnShakeHandler();
        public event OnShakeHandler onShake;

        //debug button
        public bool DebugSendShake = false;

        //debug messages
        public bool ShowDebugMessages = false;

        private float mTimeSinceLastIntervalledUpdate;
        private float mTimeSinceLastShake;
        private float mTimeSinceShakeStarted;
        private bool mIsEnabled = true;
        private bool mIsCurrentlyShaking = false;
        private float mShakeStrengthSum = 0;
        //how many single shakes have been added to the shake strength sum - needed to get the average shake strength
        private int mShakeStrengthCount = 0;


        #region UnityMethods
        private void Start()
        {
            enableShakeRecognition();
        }

        private void Update()
        {
            if (mIsEnabled)
            {
                checkShakeMovement();
            }

            if (DebugSendShake || Input.GetKeyDown(KeyCode.Return))
            {
                if (onShake != null)
                {
                    onShake();
                }
                if (ShowDebugMessages)
                    LogAR.log("A debug shake event was sent.", this, this);

                DebugSendShake = false;
            }
        }

        #endregion

        //disable detection and reset values
        public void disableShakeRecognition()
        {
            mIsEnabled = false;
        }

        public void enableShakeRecognition()
        {
            mIsEnabled = true;
            mTimeSinceLastIntervalledUpdate = UpdateIntervalInSeconds;
            mTimeSinceLastShake = PauseBetweenShakesInSeconds;
            mTimeSinceShakeStarted = 0;
        }

        private void cancelCurrentShake()
        {
            mIsCurrentlyShaking = false;
            mTimeSinceShakeStarted = 0;
            mShakeStrengthCount = 0;
            mShakeStrengthSum = 0;
        }

        private void checkShakeMovement()
        {
            Vector3 acceleration = Vector3.zero;

            mTimeSinceLastIntervalledUpdate += Time.deltaTime;
            mTimeSinceLastShake += Time.deltaTime;
            if (mIsCurrentlyShaking)
                mTimeSinceShakeStarted += Time.deltaTime;

            if (mTimeSinceLastIntervalledUpdate < UpdateIntervalInSeconds)
                return;
            mTimeSinceLastIntervalledUpdate = 0;

            if (mTimeSinceLastShake < PauseBetweenShakesInSeconds)
                return;

            acceleration = new Vector3(Input.acceleration.x, 0, -Input.acceleration.z);

            float strength = 0;
            //calculate strength of acceleration considering the gravitational force
            //(the gravity seems to be 1 in this context since 1 is the value we get when the phone lays still)
            //if acceleration equals the zero vector, we are in unity play mode without any device, hence not getting any acceleration events
            //gravity should therefore not be subtracted from the vector in this case
            if (acceleration != Vector3.zero)
            {
                strength = acceleration.magnitude - 1;
                strength *= (strength < 0) ? -1 : 1;
            }

            //a currently active shake doesn't have to be above the threshold in every update!
            // if there is no active shake and this particular shake is below the threshold - ignore it
            if (!mIsCurrentlyShaking && !((strength) >= ShakeStrengthThreshold))
            {
                return;
            }



            if (mIsCurrentlyShaking)
            {
                mShakeStrengthCount++;
                mShakeStrengthSum += strength;

                if (mTimeSinceShakeStarted >= MinShakeLengthInSeconds)
                {
                    float averageShakeStrength = mShakeStrengthSum / mShakeStrengthCount;
                    if (averageShakeStrength >= ShakeStrengthThreshold)
                    {
                        //a valid shake was recognized
                        mTimeSinceLastShake = 0;
                        mIsCurrentlyShaking = false;
                        mTimeSinceShakeStarted = 0;
                        mShakeStrengthCount = 0;
                        mShakeStrengthSum = 0;

                        if (onShake != null)
                        {
                            onShake();
                        }

                        if (ShowDebugMessages)
                        {
                            LogAR.logWarning("SHAKE: " + averageShakeStrength, this);
                        }
                    }
                    else
                    {
                        if (ShowDebugMessages)
                            LogAR.log("Current Shake was cancelled, average strength was: " + averageShakeStrength, this, this);

                        cancelCurrentShake();

                    }

                }
            }
            else
            {
                mIsCurrentlyShaking = true;
                mTimeSinceShakeStarted = 0;

                if (ShowDebugMessages)
                    LogAR.log("Started shaking...", this, this);
            }
        }
    }
}