using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace ToolbAR.Vuforia
{

    [CustomEditor(typeof(ARCameraBehaviour), true)]
    public class ARCameraBehaviourEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            if (this.target.GetType() != typeof(ARCameraBehaviour))
                return;
            ARCameraBehaviour target = (ARCameraBehaviour)this.target;

            DrawDefaultInspector();

            target.IsUsingCameraDevice = EditorGUILayout.Toggle("Is Using Camera Device", target.IsUsingCameraDevice);

        }
    }
}