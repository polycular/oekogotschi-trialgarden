using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace ToolbAR.Vuforia
{
    [CustomEditor(typeof(ARSceneBehaviour), true)]
    public class ARSceneBehaviourEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            if (this.target.GetType() != typeof(ARSceneBehaviour))
                return;
            //ARSceneBehaviour target = (ARSceneBehaviour)this.target;
            DrawDefaultInspector();
        }

        [MenuItem("ToolbAR/AR/Initialize Scene")]
        static void initializeARScene(MenuCommand cmd)
        {
            GameObject go = new GameObject("ARScene");
            go.AddComponent<ARSceneBehaviour>();
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
        }
        [MenuItem("ToolbAR/AR/Initialize Scene", true)]
        static bool canInitializeARScene(MenuCommand cmd)
        {
            return (ARScene.GameObject == null);
        }


        static GameObject _createCamera()
        {
            if (GameObject.FindObjectOfType<ARCameraBehaviour>() == null)
            {
                GameObject go = new GameObject("ARCamera");
                go.AddComponent<ARCameraBehaviour>();
                go.camera.nearClipPlane = 0.01f;
                go.camera.tag = "MainCamera";
                go.AddComponent<GUILayer>();
                go.AddComponent<AudioListener>();

                go.transform.parent = ARScene.GameObject.transform;
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);

                go.AddComponent<WebCamBehaviour>().RenderTextureLayer = 30;
                go.AddComponent<DefaultInitializationErrorHandler>();
                return go;
            }
            else return null;
        }

        [MenuItem("ToolbAR/AR/Use DataSet", true)]
        static bool canUseARScene(MenuCommand cmd)
        {
            return (ARScene.GameObject != null);
        }
        [MenuItem("ToolbAR/AR/Use DataSet")]
        static void createDataSet(MenuCommand cmd)
        {
            GameObject go = new GameObject("ARDataSet");
            go.AddComponent<ARDataSetBehaviour>();
            go.transform.parent = ARScene.GameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            UnityUtils.selectObject(go);
        }
    }
}