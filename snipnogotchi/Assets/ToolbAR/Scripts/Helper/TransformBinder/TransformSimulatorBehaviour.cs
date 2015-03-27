using UnityEngine;
using System.Collections;

namespace ToolbAR
{
    namespace ObjectBinding
    {
        public class TransformSimulatorBehaviour : MonoBehaviour
        {
            public bool IsTranslating = false;
            public bool IsRotating = false;
            public bool IsScaling = false;

            // Use this for initialization
            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {
                if (IsTranslating)
                {
                    transform.position = new Vector3(Mathf.PingPong(Time.time, 3),0,0);
                }
                if (IsRotating)
                {
                    //transform.Rotate(new Vector3(Mathf.PingPong(Time.time, 3), Mathf.PingPong(Time.time, 3), Mathf.PingPong(Time.time, 3)));
                    transform.Rotate(new Vector3(0,0,1));
                }
                if (IsScaling)
                { 
                    transform.localScale = (new Vector3(Mathf.PingPong(Time.time, 1), Mathf.PingPong(Time.time, 1), Mathf.PingPong(Time.time, 1)));
                }
            }
        }
    }
}
