using UnityEngine;
using System.Collections;

namespace ToolbAR.SceneManagement
{
    public class PrintSceneListBehaviour : MonoBehaviour
    {

        public SceneRef Scene = null;

        void Start()
        {
            //Implicit cast possible
            Scene scene = Scene;
            Debug.Log(scene == null);
        }

        // Update is called once per frame
        void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 12;

            string txt = "";
            foreach (Scene scene in SceneManager.Instance.Scenes)
            {
                txt += scene.Index + ((SceneManager.Instance.CurrentScene == scene) ? "> " : ": ") + scene.Name + " [" + scene.GUID + "]\n";
                txt += "\t" + scene.Path +"\n";
            }
            GUI.Label(new Rect(0, 0, Screen.width, SceneManager.Instance.Scenes.Length * 30 + 50), txt);
        }
    }
}