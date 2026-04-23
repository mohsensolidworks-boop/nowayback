using Main.Context.Scenes.Common.UI;
using Main.Context.Scenes.Common.General;
using Main.Infrastructure.Context;
using Main.Infrastructure.General;
using UnityEngine;

namespace Main.Context.Scenes.Gameplay.UI
{
    public class GameplayUI : ManualBehaviour
    {
        [SerializeField] private Transform _pauseButtonContainer;
        
        public void Init()
        {
            var cameraManager = Contexts.Get<CameraManager>();
            var cameraWidthHalf = cameraManager.Width / cameraManager.SafeHeightRatio / 100f / 2f;
            var cameraHeightHalf = cameraManager.Height / cameraManager.SafeHeightRatio / 100f / 2f;
            _pauseButtonContainer.position = new Vector3(cameraWidthHalf - 1f, cameraHeightHalf - 1f, 0f);
        }
        
        public void ClickPauseButton()
        {
            var dialogManager = Contexts.Get<DialogManager>();
            dialogManager.OpenDialog(DialogType.PauseDialog);
        }
    }
}
