using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Main.Editor.EditorTools
{
    public abstract class AEditorToolsWindow : EditorWindow
    {
        private string _foldStateMaskKey => GetWindowName() + "FoldStatePrefs";
        private int _foldStateMask;
        private Vector2 _scrollPos;
        private readonly List<AEditorTool> _tools = new();
        
        protected abstract string GetWindowName();
        
        protected static void ShowWindow<T>(string name) where T : AEditorToolsWindow
        {
            var window = GetWindow<T>();
            window.titleContent = new GUIContent(name);
            window.Show();
        }
        
        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            DrawTools();
            EditorGUILayout.EndScrollView();
        }
        
        private void DrawTools()
        {
            if (_tools.Count == 0 || _tools[0] == null)
            {
                CreateTools();
            }
            
            for (var i = 0; i < _tools.Count; i++)
            {
                _tools[i].DrawTool();
            }
        }

        private void CreateTools()
        {
            _tools.Clear();
            
            var toolTypes = GetToolTypes();
            for (var i = 0; i < toolTypes.Length; i++)
            {
                _tools.Add((AEditorTool)CreateInstance(toolTypes[i]));
            }
            
            _foldStateMask = EditorPrefs.GetInt(_foldStateMaskKey, 0);
            
            for (var i = 0; i < _tools.Count; i++)
            {
                var foldState = (_foldStateMask & 1 << i) != 0;
                _tools[i].Init(this, foldState);
                PrepareTool(_tools[i]);
            }
        }
        
        protected abstract Type[] GetToolTypes();
        
        protected virtual void PrepareTool(AEditorTool tool)
        {
        }
        
        public void UpdateFoldStateMask()
        {
            var newMask = 0;
            for (var i = 0; i < _tools.Count; i++)
            {
                if (_tools[i].FoldState)
                {
                    newMask |= 1 << i;
                }
            }
            
            _foldStateMask = newMask;
            EditorPrefs.SetInt(_foldStateMaskKey, _foldStateMask);
        }
    }
}
