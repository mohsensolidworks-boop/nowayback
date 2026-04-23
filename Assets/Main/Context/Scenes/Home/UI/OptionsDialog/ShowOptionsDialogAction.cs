using System;
using Main.Context.Scenes.Common.UI;

namespace Main.Context.Scenes.Home.UI.OptionsDialog
{
    public class ShowOptionsDialogAction : DialogAction
    {
        public ShowOptionsDialogAction(DialogType dialogType, Action onComplete = null) : base(dialogType, onComplete)
        {
        }
    }
}
