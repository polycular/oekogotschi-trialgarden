using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ToolbAR.SceneManagement
{
    /// <summary>
    /// This Class handles the Serialization and Deserialization, as well as other Editor Counterparts, of the SceneManager
    /// </summary>
    public static class SceneManagerEditor
    {
        /// <summary>
        /// Evertime Unity Builds (or plays), this class will be reset to defaults.
        /// As handlePostprocessScene could be clled multiple times in a build, we will capture this by the setting this flag
        /// to true once, cancelling multiple buildings of the SceneManager
        /// </summary>
        private static bool mHasBuilt = false;

        [PostProcessBuild]
        public static void handlePostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            buildSceneManager(true);
        }
        [PostProcessScene]
        public static void handlePostprocessScene()
        {
            buildSceneManager(false);
        }

        static void buildSceneManager(bool releaseBuild)
        {
            if (!mHasBuilt)
            {
                SceneManager.saveToStreamingAssets(Scene.fetchAllFromBuildSettings(), releaseBuild);
            }
        }
    }
}
