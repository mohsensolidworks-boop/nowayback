using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Main.Editor
{
    [CustomEditor(typeof(ScriptableObject), true)]
    public class NestedScriptableObjectEditor : UnityEditor.Editor
    {
        private const string _EXPANDED_STATES_KEY = "NestedSOEditor_ExpandedStates";
        private static Dictionary<string, bool> _EXPANDED_STATES;
        private readonly Dictionary<string, UnityEditor.Editor> _nestedEditors = new();
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            var property = serializedObject.GetIterator();
            if (property.NextVisible(true))
            {
                do
                {
                    if (property.name == "m_Script")
                    {
                        GUI.enabled = false;
                        EditorGUILayout.PropertyField(property, true);
                        GUI.enabled = true;
                        continue;
                    }
                    
                    if (property.propertyType == SerializedPropertyType.ObjectReference &&
                        property.objectReferenceValue is ScriptableObject nestedSo)
                    {
                        var key = $"{target.GetInstanceID()}_{property.propertyPath}";
                        if (!_EXPANDED_STATES.TryGetValue(key, out bool isExpanded))
                        {
                            isExpanded = false;
                        }
                        
                        EditorGUILayout.BeginVertical("box");
                        EditorGUILayout.BeginHorizontal();
                        var buttonLabel = isExpanded ? "▼" : "▶";
                        
                        var prevBackground = GUI.backgroundColor;
                        if (isExpanded)
                        {
                            GUI.backgroundColor = Color.yellow;
                        }
                        
                        if (GUILayout.Button(buttonLabel, GUILayout.Width(20)))
                        {
                            isExpanded = !isExpanded;
                            _EXPANDED_STATES[key] = isExpanded;
                            SaveExpandedStates();
                        }
                        GUI.backgroundColor = prevBackground;

                        EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(property.name), GUILayout.Width(110));
                        EditorGUILayout.PropertyField(property, GUIContent.none, true);
                        EditorGUILayout.EndHorizontal();

                        if (isExpanded)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.BeginVertical();
                            if (!_nestedEditors.TryGetValue(property.propertyPath, out var nestedEditor) ||
                                nestedEditor.target != nestedSo)
                            {
                                nestedEditor = CreateEditor(nestedSo);
                                _nestedEditors[property.propertyPath] = nestedEditor;
                            }
                            
                            if (nestedEditor != null)
                            {
                                nestedEditor.OnInspectorGUI();
                            }
                            EditorGUILayout.EndVertical();
                            EditorGUI.indentLevel--;
                        }
                        EditorGUILayout.EndVertical();
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(property, true);
                    }
                }
                while (property.NextVisible(false));
            }
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void OnEnable()
        {
            LoadExpandedStates();
        }
        
        private static void LoadExpandedStates()
        {
            if (EditorPrefs.HasKey(_EXPANDED_STATES_KEY))
            {
                var json = EditorPrefs.GetString(_EXPANDED_STATES_KEY, "{}");
                _EXPANDED_STATES = JsonUtility.FromJson<BoolDictionaryWrapper>(json)?.ToDictionary() ?? new Dictionary<string, bool>();
            }
            else
            {
                _EXPANDED_STATES = new Dictionary<string, bool>();
            }
        }
        
        private static void SaveExpandedStates()
        {
            var wrapper = new BoolDictionaryWrapper(_EXPANDED_STATES);
            var json = JsonUtility.ToJson(wrapper);
            EditorPrefs.SetString(_EXPANDED_STATES_KEY, json);
        }
        
        [System.Serializable]
        private class BoolDictionaryWrapper
        {
            public List<string> Keys = new();
            public List<bool> Values = new();
            
            public BoolDictionaryWrapper()
            {
            }
            
            public BoolDictionaryWrapper(Dictionary<string, bool> dict)
            {
                Keys = dict.Keys.ToList();
                Values = dict.Values.ToList();
            }
            
            public Dictionary<string, bool> ToDictionary()
            {
                var dict = new Dictionary<string, bool>();
                for (var i = 0; i < Mathf.Min(Keys.Count, Values.Count); i++)
                {
                    dict[Keys[i]] = Values[i];
                }
                
                return dict;
            }
        }
    }
}