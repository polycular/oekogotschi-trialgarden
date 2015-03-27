using UnityEngine;
using System.Collections;

namespace ToolbAR
{
    public class FaceCameraBehaviour : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            transform.LookAt(Camera.main.transform.position, Vector3.up);

        }
    }
}