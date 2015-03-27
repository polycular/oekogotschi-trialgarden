using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

using ToolbAR.CSharpExtensions;

namespace ToolbAR.SceneManagement
{
    [JsonObject(MemberSerialization.OptIn)]
    /// <summary>
    /// A Scene is an in-code Represntation of a Reference to a Unity Scene in its build settings.
    /// It contaisn helpers and all needed information about a scene.
    /// As it is managed through the SceneManager, it is unique and cannot be copied.
    /// This also means, that event listeners for specific scenes are possible
    /// </summary>
    public class Scene
    {
        #region Public Events

        /// <summary>
        /// Fired on the old scene, exactly before SceneManager.beforeSceneChange is fired
        /// </summary>
        public event SceneManager.BeforeSceneChangeHandler beforeLeave;

        /// <summary>
        /// Fired on the new scene, exactly before SceneManager.beforeSceneChange is fired, but after beforeLeave on the old scene was fired.
        /// </summary>
        public event SceneManager.BeforeSceneChangeHandler beforeSceneChangeTo;
        /// <summary>
        /// Fired on the new scene, exactly after SceneManager.afterSceneChange is fired
        /// </summary>
        public event SceneManager.AfterSceneChangeHandler afterSceneChangeTo;

        #endregion

        public override string ToString()
        {
            return base.ToString() + " (\"" + mPath + "\" :: "+mGUID+" )";
        }

        public static explicit operator Scene(SceneRef r)
        {
            return r.resolve();
        }

        #region Public Properties

        public int Index
        {
            get
            {
                return mIdx;
            }
        }

        /// <summary>
        /// Path is Relative to the old Assets/ Folder before build
        /// </summary>
        public string Path
        {
            get
            {
                return mPath;
            }
        }

        public string Name
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(mPath);
            }
        }

        public string GUID
        {
            get
            {
                return mGUID;
            }
        }

        #endregion

        #region public Methods
        public void emitBeforeSceneLeaveEvent(Scene oldScene, Scene newScene)
        {
            if (beforeLeave != null)
            {
                beforeLeave(oldScene, newScene);
            }
        }
        public void emitBeforeSceneChangeToEvent(Scene oldScene, Scene newScene)
        {
            if (beforeSceneChangeTo != null)
            {
                beforeSceneChangeTo(oldScene, newScene);
            }
        }
        public void emitAfterSceneChangeToEvent(Scene oldScene, Scene newScene)
        {
            if (afterSceneChangeTo != null)
            {
                afterSceneChangeTo(oldScene, newScene);
            }
        }

        /// <summary>
        /// Relays the Call to the SceneManager to load/change to this scene
        /// </summary>
        public void load()
        {
            SceneManager.Instance.changeScene(this);
        }

        #endregion

        #region Editor-Only Methods
#if UNITY_EDITOR
        static public Scene[] fetchAllFromBuildSettings()
        {
            List<Scene> scenes = new List<Scene>();
            int i = 0;
            //Walk through the build settings again to get the index of this
            foreach (var buildSetting in UnityEditor.EditorBuildSettings.scenes)
            {
                if (buildSetting.enabled)
                {
                    Scene scene = new Scene();
                    scene.mIdx = i;

                    scene.mPath = PathExtensions.getRelativePathTo(System.IO.Path.GetFullPath(buildSetting.path), Application.dataPath+"/");

                    scene.mGUID = UnityEditor.AssetDatabase.AssetPathToGUID(buildSetting.path);
                    i++;
                    scenes.Add(scene);
                }
            }
            return scenes.ToArray();
        }
#endif

        #endregion

        #region Private Fields

        [JsonProperty]
        int mIdx;
        [JsonProperty]
        string mPath;
        [JsonProperty]
        string mGUID;

        #endregion

        #region Private Methods

        [JsonConstructor]
        private Scene()
        {
        }

        #endregion
    }
}