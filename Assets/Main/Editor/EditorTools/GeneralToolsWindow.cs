using System;
using UnityEditor;

namespace Main.Editor.EditorTools
{
    public class GeneralToolsWindow : AEditorToolsWindow
    {
        private const string _WINDOW_NAME = "General Tools";
        
        protected override string GetWindowName()
        {
            return _WINDOW_NAME;
        }
        
        [MenuItem("Tools/" + _WINDOW_NAME)]
        protected static void ShowWindow()
        {
            ShowWindow<GeneralToolsWindow>(_WINDOW_NAME);
        }
        
        protected override Type[] GetToolTypes()
        {
            return new[]
            {
                typeof(SettingsEditorTool),
                typeof(ParametersEditorTool),
            };
        }
    }
}
