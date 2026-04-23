using System;
using UnityEditor;
using UnityEngine;

namespace Main.Editor.EditorTools
{
    public abstract class AEditorTool : ScriptableObject
    {
        protected abstract string _Header { get; }
        
        public AEditorToolsWindow ParentWindow;
        
        private const int _DEFAULT_COLUMN_WIDTH = 180;
        private const int _DEFAULT_WINDOW_WIDTH = 360;
        private const int _DEFAULT_BUTTON_HEIGHT = 25;
        
        protected readonly GUILayoutOption[] _DefaultColumnOption =
        {
            GUILayout.Width(_DEFAULT_COLUMN_WIDTH)
        };
        
        protected readonly GUILayoutOption[] _DefaultButtonOption =
        {
            GUILayout.Width(_DEFAULT_COLUMN_WIDTH), GUILayout.Height(_DEFAULT_BUTTON_HEIGHT)
        };
        
        protected readonly GUILayoutOption[] _BoldButtonOption =
        {
            GUILayout.Width(_DEFAULT_COLUMN_WIDTH), GUILayout.Height(_DEFAULT_BUTTON_HEIGHT * 1.5f)
        };
        
        protected readonly GUILayoutOption[] _ThinButtonOption =
        {
            GUILayout.Width(_DEFAULT_WINDOW_WIDTH), GUILayout.Height(_DEFAULT_BUTTON_HEIGHT / 1.5f)
        };
        
        public bool FoldState;
        
        public void Init(AEditorToolsWindow parentWindow, bool foldState)
        {
            ParentWindow = parentWindow;
            FoldState = foldState;
        }
        
        public void DrawTool()
        {
            if (DrawHeader())
            {
                DrawGUI();
            }
        }
        
        private bool DrawHeader()
        {
            GUILayout.BeginHorizontal("box");
            
            var headerStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
            
            var newFoldState = EditorGUILayout.Foldout(FoldState, _Header, true, headerStyle);
            if (newFoldState != FoldState)
            {
                FoldState = newFoldState;
                ParentWindow.UpdateFoldStateMask();
            }
            
            GUILayout.EndHorizontal();
            
            return FoldState;
        }
        
        protected abstract void DrawGUI();
        
        protected bool DrawButton(string text)
        {
            return DrawButton(text, _DefaultButtonOption);
        }
        
        protected bool DrawButton(string text, GUILayoutOption[] layoutOption)
        {
            return DrawButton(new GUIContent(text), layoutOption);
        }
        
        protected bool DrawButton(string text, float width, float height)
        {
            return DrawButton(new GUIContent(text), width, height);
        }
        
        protected bool DrawButton(GUIContent content, float width, float height)
        {
            var layoutOption = new[] { GUILayout.Width(width), GUILayout.Height(height) };
            return DrawButton(content, layoutOption);
        }
        
        private bool DrawButton(GUIContent content, GUILayoutOption[] layoutOption)
        {
            return GUILayout.Button(content, layoutOption);
        }

        protected void Column(bool isBox, Action content)
        {
            GUILayout.BeginVertical(!isBox ? GUIStyle.none : EditorStyles.helpBox, GUILayout.MinWidth(_DEFAULT_COLUMN_WIDTH));
            content();
            GUILayout.EndVertical();
        }

        protected void Row(Action content)
        {
            GUILayout.BeginHorizontal();
            // GUILayout.FlexibleSpace();
            content();
            // GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
