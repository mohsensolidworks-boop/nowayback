using UnityEditor;
using UnityEngine;

namespace Main.Editor.EditorTools
{
    public class ParametersEditorTool : AEditorTool
    {
        protected override string _Header => "Parameters";
        
        protected override void DrawGUI()
        {
            Column(true, () =>
            {
                Row(() =>
                {
                    EditorGUI.BeginChangeCheck();
                    
                    var newGravity = EditorGUILayout.Slider("Gravity", Physics.gravity.y, -50, 0);
                    
                    if (EditorGUI.EndChangeCheck())
                    {
                        var dynamicsManager = AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/DynamicsManager.asset");
                        if (dynamicsManager != null)
                        {
                            Undo.RecordObject(dynamicsManager, "Gravity");
                            EditorUtility.SetDirty(dynamicsManager);
                        }
                        
                        Physics.gravity = new Vector3(0f, newGravity, 0f);
                    }
                });
            });
        }
    }
}
