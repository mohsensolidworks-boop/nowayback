using UnityEditor;
using UnityEditor.SceneManagement;

namespace Main.Editor.EditorTools
{
    [InitializeOnLoad]
    public class Startup
    {
        private const string _LAUNCH_START_KEY = "launchStartScene";

        public static bool LAUNCH_START_SCENE
        {
            get
            {
                return EditorPrefs.GetBool(_LAUNCH_START_KEY, true);
            }
            set
            {
                EditorPrefs.SetBool(_LAUNCH_START_KEY, value);
                ArrangeLaunchScene();
            }
        }
        
        static Startup()
        {
            ArrangeLaunchScene();
        }
        
        private static void ArrangeLaunchScene()
        {
            SceneAsset startScene = null;
            if (LAUNCH_START_SCENE)
            {
                startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
            }

            EditorSceneManager.playModeStartScene = startScene;
        }
    }
}
