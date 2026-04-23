using System;
using Main.Context.Scenes.Common.UI;

namespace Main.Context.Scenes.Gameplay.UI.PauseDialog
{
    public class ShowPauseDialogAction : DialogAction
    {
        public ShowPauseDialogAction(DialogType dialogType, Action onComplete = null) : base(dialogType, onComplete)
        {
        }
    }
}
