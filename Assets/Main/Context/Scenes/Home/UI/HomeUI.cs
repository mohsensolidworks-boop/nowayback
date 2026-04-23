using Main.Context.Core.General;
using Main.Context.Scenes.Common.General;
using Main.Context.Scenes.Common.UI;
using Main.Context.Scenes.Home.UI.Assets.Materials.Displacement;
using Main.Infrastructure.Context;
using Main.Infrastructure.General;
using TMPro;
using UnityEngine;

namespace Main.Context.Scenes.Home.UI
{
    public class HomeUI : ManualBehaviour
    {
        [SerializeField] private TextMeshPro _playButtonText;
        [SerializeField] private GameObject _adminButton;
        [SerializeField] private ShaderMouseControlExperiment _shaderMouseControlExperiment;
        
        private InputManager _inputManager;
        private float _touchDownTime;
        private bool _isTouchedOnce;
        
        public void Init()
        {
            _inputManager = Contexts.Get<InputManager>();
            
            _shaderMouseControlExperiment.Init();
            Refresh();
        }
        
        public void Refresh()
        {
        }
        
        public void ClickPlayButton()
        {
            Contexts.Get<SceneChangeManager>().LoadGameplayScene();
        }

        public void ClickOptionsButton()
        {
            var dialogManager = Contexts.Get<DialogManager>();
            dialogManager.OpenDialog(DialogType.OptionsDialog);
        }
        
        public void ClickExitButton()
        {
            Application.Quit();
        }
        
        public void ClickAdminButton()
        {
            var dialogManager = Contexts.Get<DialogManager>();
            dialogManager.OpenDialog(DialogType.AdminDialog);
        }
        
        public void ManualUpdate()
        {
            if (_inputManager.GetTouchUp())
            {
                if (_isTouchedOnce)
                {
                    var isAdminActive = _adminButton.activeSelf;
                    _adminButton.SetActive(!isAdminActive);
                }
                else
                {
                    _isTouchedOnce = true;
                    _touchDownTime = 0.5f;
                }
            }
            
            if (_isTouchedOnce)
            {
                _touchDownTime -= Time.deltaTime;
                if (_touchDownTime <= 0)
                {
                    _isTouchedOnce = false;
                }
            }
        }
    }
}
