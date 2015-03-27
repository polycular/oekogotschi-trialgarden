using UnityEngine;
using System.Collections;

namespace ToolbAR.SceneManagement
{
    [System.Serializable]
    /// <summary>
    /// Class tht Helps keeping track of Scenes 'set' over the editor
    /// To avoid Typos inplain strings, this is a custom Serializable Class,
    /// supported by a Unity PropertyDrawer
    /// </summary>
    public class SceneRef
    {
        [SerializeField]
        private string mPath;
        public string Path
        {
            get
            {
                return mPath;
            }
        }
        public bool IsInvalid
        {
            get
            {
                if (mIsInvalid == false && mResolvedScene == null)
                    resolve();
                return mIsInvalid;
            }
        }
        public bool IsValid
        {
            get
            {
                if (mIsInvalid == false && mResolvedScene == null)
                    resolve();
                return !mIsInvalid;
            }
        }
        /// <summary>
        /// Actual just a syntactic relay to resolve()
        /// </summary>
        public Scene Scene
        {
            get
            {
                return resolve();
            }
        }

        private Scene mResolvedScene = null;
        private bool mIsInvalid = false;

        /// <summary>
        /// Resolves this SceneRef to a Scene, caching and reruning the result
        /// </summary>
        /// <returns></returns>
        public Scene resolve()
        {
            if (mIsInvalid)
                return null;
            if(mPath.Length == 0)
            {
                mIsInvalid = true;
                return null;
            }
            if (mResolvedScene == null)
            {
                mResolvedScene = SceneManager.Instance.getSceneByPath(mPath);
                if (mResolvedScene == null)
                    mIsInvalid = true;
            }
            return mResolvedScene;
        }

        public override string ToString()
        {
            return base.ToString() + " to \n\t" + Scene;
        }

        public static implicit operator Scene(SceneRef s)
        {
            return s.resolve();
        }

        public static SceneRef fromScene(Scene scene)
        {
            if (scene == null)
                return null;
            SceneRef sceneref = new SceneRef();
            sceneref.mPath = scene.Path;
            sceneref.mResolvedScene = scene;

            return sceneref;
        }
    }
}