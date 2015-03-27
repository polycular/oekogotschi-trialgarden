using UnityEngine;
using System.Collections;

namespace ToolbAR.Sandbox
{
    public class ARScene2 : MonoBehaviour
    {
        public bool DBG_loadOtherLevel = false;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (DBG_loadOtherLevel)
            {
                DBG_loadOtherLevel = true;

                Application.LoadLevel("ARScene2_2");
                
            }
        }
    }
}