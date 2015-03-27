using UnityEngine;
using System.Collections;

namespace ToolbAR
{
    namespace ObjectBinding
    {
        // * This Behaviour shall be placed on the transform pantomime.

        public class TransformBindBehaviour : MonoBehaviour
        {
            public bool IsCloningTranslation;
            public bool IsCloningRotation;
            public bool IsCloningScale;

            public bool IsShadowingTranslation;
            public bool IsShadowingRotation;
            public bool IsShadowingScale;

            public GameObject master;

            private Vector3? mMasterLastPosition;
            private Vector3? mMasterLastRotation;
            private Vector3? mMasterLastScale;

            #region UnityMethods

            void Start()
            {
                if (master == null)
                {
                    LogAR.logError(GetType().Name + " - " + "master is null",this);
                }
            }

            void Update()
            {
                if (master != null)
                {
                    //cloning operations
                    if (IsCloningTranslation)
                    {
                        transform.position = master.transform.position;
                    }
                    if (IsCloningRotation)
                    {
                        transform.rotation = master.transform.rotation;
                    }
                    if (IsCloningScale)
                    {
                        transform.localScale = master.transform.localScale;
                    }

                    if (!mMasterLastPosition.HasValue)
                    {
                        mMasterLastPosition = master.transform.position;
                    }
                    if (!mMasterLastRotation.HasValue)
                    {
                        mMasterLastRotation = master.transform.eulerAngles;
                    }
                    if (!mMasterLastScale.HasValue)
                    {
                        mMasterLastScale = master.transform.localScale;
                    }

                    //shadowing operations
                    if (IsShadowingTranslation)
                    {
                        Vector3 actualTransform = master.transform.position;
                        Vector3 delta = actualTransform - (Vector3)mMasterLastPosition;
                        transform.position += delta;
                        mMasterLastPosition = actualTransform;
                    }
                    if (IsShadowingRotation)
                    {
                        const float unityEulerAngleThreshold = 359.5f;

                        Vector3 actualEulerRotation = master.transform.eulerAngles;
                        Vector3 delta = actualEulerRotation - (Vector3)mMasterLastRotation;

                        transform.eulerAngles += new Vector3(delta.x % unityEulerAngleThreshold,
                                                             delta.y % unityEulerAngleThreshold,
                                                             delta.z % unityEulerAngleThreshold);
                        mMasterLastRotation = actualEulerRotation;
                    }
                    if (IsShadowingScale)
                    {
                        Vector3 actualLocalScale = master.transform.localScale;
                        Vector3 delta = actualLocalScale - (Vector3)mMasterLastScale;
                        transform.localScale += delta;
                        mMasterLastScale = actualLocalScale;
                    }
                }
            }
            #endregion
        }
    }
}
