using Main.Infrastructure.Context;
using Main.Context.Scenes.Common.UI;
using UnityEngine;

namespace Main.Context.Scenes.Common.General
{
    public sealed class InputListener : ISceneContextUnit
    {
        public bool WasPauseDialogOpen { get; private set; }
        
        private DialogManager _dialogManager;
        private InputManager _inputManager;
        
        public void Bind()
        {
            _dialogManager = Contexts.Get<DialogManager>();
            _inputManager = Contexts.Get<InputManager>();
        }
        
        public void ControlInputs()
        {
            WasPauseDialogOpen = _dialogManager.IsDialogOpen(DialogType.PauseDialog);
            
            if (_inputManager.GetKeyUp(KeyCode.Escape))
            {
                _dialogManager.PressBack();
            }
        }
    }
}
