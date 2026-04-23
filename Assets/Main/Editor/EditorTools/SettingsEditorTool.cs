using System.IO;
using System.Linq;
using Main.Infrastructure.Utils;
using UnityEditor;
using UnityEngine;

namespace Main.Editor.EditorTools
{
    public class SettingsEditorTool : AEditorTool
    {
        protected override string _Header => "Settings";
        
        protected override void DrawGUI()
        {
            Column(true, () =>
            {
                Row(() =>
                {
                    if (DrawButton("Launch From Start"))
                    {
                        Startup.LAUNCH_START_SCENE = !Startup.LAUNCH_START_SCENE;
                    }
                    
                    Startup.LAUNCH_START_SCENE = EditorGUILayout.Toggle("", Startup.LAUNCH_START_SCENE, GUILayout.Width(14.8f));
                });
            });
            
            Column(true, () =>
            {
                Row(() =>
                {
                    if (DrawButton("Clear Local Data"))
                    {
                        RemoveLocalData();
                    }
                });
                
                Row(() =>
                {
                    if (DrawButton("Remove Empty Folders"))
                    {
                        RemoveEmptyFolders();
                    }
                });
            });
        }
        
        private static void RemoveLocalData()
        {
            PlayerPrefs.DeleteAll();
            Directory.Delete(FileHelper.PERSISTENT_DATA_PATH, true);
        }
        
        private void RemoveEmptyFolders()
        {
            RemoveEmptyDirsRecursively(Application.dataPath);
            AssetDatabase.Refresh();
        }
        
        private static void RemoveEmptyDirsRecursively(string startLocation)
        {
            var dirs = Directory.GetDirectories(startLocation);
            foreach (var directory in dirs)
            {
                RemoveEmptyDirsRecursively(directory);
                var files = Directory.GetFiles(directory)
                    .Where(file => !file.EndsWith(".meta") && !file.EndsWith(".DS_Store")).ToList();
                var dirCount = Directory.GetDirectories(directory).Length;
                if (files.Count == 0 && dirCount == 0)
                {
                    Directory.Delete(directory, true);
                    var metaFile = directory + ".meta";
                    if (File.Exists(metaFile))
                    {
                        File.Delete(metaFile);
                    }
                }
            }
        }
    }
}
