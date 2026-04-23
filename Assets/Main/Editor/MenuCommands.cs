using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Main.Editor
{
    public static class MenuCommands
    {
        private const string _MAIN_MENU = "Tools";
        private static TransformData _TRANSFORM_HOLDER;
        
        [MenuItem(_MAIN_MENU + "/Utils/Clear Console _c")]
        public static void ClearConsole()
        {
            var assembly = Assembly.GetAssembly(typeof(SceneView));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            if (method != null)
            {
                method.Invoke(new object(), null);
            }
        }
        
        [MenuItem(_MAIN_MENU + "/Utils/Take Screenshot &s")]
        public static void TakeScreenshot()
        {
            var folderPath = "Screenshots";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            ScreenCapture.CaptureScreenshot(Path.Combine(folderPath, $"{Time.realtimeSinceStartup}.png"));
        }
        
        [MenuItem(_MAIN_MENU + "/Utils/Crash Game &#c")]
        public static void CrashGame()
        {
            if (EditorApplication.isPlaying)
            {
                Utils.ForceCrash(ForcedCrashCategory.Abort);
            }
        }
        
        [MenuItem(_MAIN_MENU + "/Utils/Time Scale x0.1 _1")]
        public static void SetTimeScale1()
        {
            if (EditorApplication.isPlaying)
            {
                Time.timeScale = 0.1f;
            }
        }
        
        [MenuItem(_MAIN_MENU + "/Utils/Time Scale x1.0 _2")]
        public static void SetTimeScale2()
        {
            if (EditorApplication.isPlaying)
            {
                Time.timeScale = 1f;
            }
        }
        
        [MenuItem(_MAIN_MENU + "/Utils/Time Scale x2.0 _3")]
        public static void SetTimeScale3()
        {
            if (EditorApplication.isPlaying)
            {
                Time.timeScale = 2f;
            }
        }

        [MenuItem(_MAIN_MENU + "/Utils/Time Scale x10.0 _4")]
        public static void SetTimeScale4()
        {
            if (EditorApplication.isPlaying)
            {
                Time.timeScale = 10f;
            }
        }

        [MenuItem(_MAIN_MENU + "/GameObject/Toggle GameObjects &a")]
        public static void ToggleGameObjects()
        {
            var gameObjects = Selection.gameObjects;
            if (gameObjects != null)
            {
                for (var i = 0; i < gameObjects.Length; i++)
                {
                    var gameObject = gameObjects[i];
                    Undo.RecordObject(gameObject, "Toggle GameObject");
                    gameObject.SetActive(!gameObject.activeSelf);
                }
            }
        }
        
        [MenuItem(_MAIN_MENU + "/GameObject/Copy Transform &c")]
        public static void CopyTransform()
        {
            var obj = Selection.activeGameObject;
            if (obj != null)
            {
                _TRANSFORM_HOLDER = new TransformData(obj.transform);
            }
        }

        [MenuItem(_MAIN_MENU + "/GameObject/Paste Transform &v")]
        public static void PasteTransform()
        {
            var obj = Selection.activeGameObject;
            if (obj != null)
            {
                Undo.RecordObject(obj.transform, "Changed Transform");
                _TRANSFORM_HOLDER.ApplyTo(obj.transform);
            }
        }

        [MenuItem(_MAIN_MENU + "/GameObject/Reset Transform %t")]
        public static void ResetTransform()
        {
            var go = Selection.activeGameObject;
            if (go != null)
            {
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
            }
        }
        
        private readonly struct TransformData
        {
            private readonly Vector3 _localPosition;
            private readonly Vector3 _localRotation;
            private readonly Vector3 _localScale;

            public TransformData(Transform transform)
            {
                _localPosition = transform.localPosition;
                _localRotation = transform.localEulerAngles;
                _localScale = transform.localScale;
            }

            public void ApplyTo(Transform transform)
            {
                transform.localPosition = _localPosition;
                transform.localEulerAngles = _localRotation;
                transform.localScale = _localScale;
            }
        }
    }
}