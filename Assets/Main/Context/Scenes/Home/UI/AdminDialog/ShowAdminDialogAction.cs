using System;
using Main.Context.Scenes.Common.UI;

namespace Main.Context.Scenes.Home.UI.AdminDialog
{
    public class ShowAdminDialogAction : DialogAction
    {
        public ShowAdminDialogAction(DialogType dialogType, Action onComplete) : base(dialogType, onComplete)
        {
        }
    }
}
