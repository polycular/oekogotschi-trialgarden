using UnityEngine;
using System.Collections;

namespace ToolbAR
{
    namespace Vuforia
    {
        public class ARSandbox : MonoBehaviour
        {
            // Use this for initialization
            void Start()
            {
                ARScene.Instance.onQCARImageTrackerInit += handleQCARImageTrackerInit;
                this.gameObject.SetActive(false);
            }

            void handleQCARImageTrackerInit(ImageTracker tracker)
            {
                this.gameObject.SetActive(true);

                //DataSet ds = ARScene.Instance.QCARImageTracker.CreateDataSet();
                //ds.Load("Default");
            }

            // Update is called once per frame
            void Update()
            {
            }
        }
    }
}