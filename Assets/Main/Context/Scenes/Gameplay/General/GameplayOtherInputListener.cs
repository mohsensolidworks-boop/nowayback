using Main.Context.Scenes.Common.General;
using Main.Context.Scenes.Common.UI;
using Main.Infrastructure.Context;
using UnityEngine;

namespace Main.Context.Scenes.Gameplay.General
{
    public sealed class GameplayOtherInputListener : ISceneContextUnit
    {
        private DialogManager _dialogManager;
        private InputManager _inputManager;
        private InputListener _inputListener;
        
        public void Bind()
        {
            _dialogManager = Contexts.Get<DialogManager>();
            _inputManager = Contexts.Get<InputManager>();
            _inputListener = Contexts.Get<InputListener>();
        }
        
        public void ControlInputs()
        {
            if (_inputListener.WasPauseDialogOpen)
            {
                return;
            }
            
            if (_inputManager.GetKeyUp(KeyCode.Escape))
            {
                _dialogManager.OpenDialog(DialogType.PauseDialog);
            }
        }
    }
}
