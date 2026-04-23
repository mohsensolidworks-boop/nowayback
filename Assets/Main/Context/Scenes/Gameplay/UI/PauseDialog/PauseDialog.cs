using Main.Context.Scenes.Common.UI;
using Main.Context.Scenes.Common.UI.Components;
using Main.Context.Scenes.Gameplay.General;
using Main.Infrastructure.Context;

namespace Main.Context.Scenes.Gameplay.UI.PauseDialog
{
    public class PauseDialog : UIDialog
    {
        public void ClickOptionsButton()
        {
            var dialogManager = Contexts.Get<DialogManager>();
            dialogManager.OpenDialog(DialogType.OptionsDialog, OnOptionsDialogClosed);
            
            Close();
        }
        
        public void ClickHomeButton()
        {
            Contexts.Get<GameplayExitManager>().RequestReturnHome(false);
        }
        
        private void OnOptionsDialogClosed()
        {
            var dialogManager = Contexts.Get<DialogManager>();
            dialogManager.OpenDialog(DialogType.PauseDialog);
        }
    }
}
