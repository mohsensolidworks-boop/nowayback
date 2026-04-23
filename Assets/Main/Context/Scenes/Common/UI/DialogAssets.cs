using System;
using Main.Context.Scenes.Common.UI.Components;
using Main.Context.Scenes.Gameplay.UI.PauseDialog;
using Main.Context.Scenes.Home.UI.AdminDialog;
using Main.Context.Scenes.Home.UI.OptionsDialog;
using UnityEngine;

namespace Main.Context.Scenes.Common.UI
{
    [CreateAssetMenu(fileName = "DialogAssets", menuName = "Scriptable Objects/DialogAssets")]
    public class DialogAssets : ScriptableObject
    {
        [field: SerializeField] public AdminDialog AdminDialog { get; private set; }
        [field: SerializeField] public PauseDialog PauseDialog { get; private set; }
        [field: SerializeField] public OptionsDialog OptionsDialog { get; private set; }
        
        public UIDialog GetDialogPrefab(DialogType dialogType)
        {
            return dialogType switch
            {
                DialogType.AdminDialog => AdminDialog,
                DialogType.PauseDialog => PauseDialog,
                DialogType.OptionsDialog => OptionsDialog,
                DialogType.None => null,
                _ => throw new ArgumentOutOfRangeException(nameof(dialogType), dialogType, null)
            };
        }
    }
}
