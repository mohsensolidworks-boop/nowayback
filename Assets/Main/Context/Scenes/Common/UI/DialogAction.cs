using System;
using Main.Context.Core.General;
using Main.Context.Scenes.Common.UI.Components;
using Main.Infrastructure.Context;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Main.Context.Scenes.Common.UI
{
    public abstract class DialogAction : FlowAction
    {
        public DialogType DialogType { get; }
        
        private UIDialog _dialog;
        private readonly DialogManager _dialogManager;
        private bool _isClosing;
        private bool _isCanceled;
        private bool _closingDialogStarted;
        
        protected DialogAction(DialogType dialogType, Action onComplete)
        {
            DialogType = dialogType;
            OnCompleted += onComplete;
            _dialogManager = Contexts.Get<DialogManager>();
        }
        
        public override void Execute()
        {
            _dialogManager.ShowDialog(this);
        }
        
        public void ShowDialog(SortingData dialogSorting, Transform dialogParent)
        {
            var assetManager = Contexts.Get<AssetManager>();
            var dialogAssets = assetManager.GetDialogAssets();
            var dialogPrefab = dialogAssets.GetDialogPrefab(DialogType);
            _dialog = Object.Instantiate(dialogPrefab);
            _dialog.StartShow(this, dialogSorting, dialogParent);
        }
        
        public void CancelDialog()
        {
            if (_isCanceled)
            {
                return;
            }
            
            _isCanceled = true;
            
            if (_dialog != null)
            {
                _dialog.StartCancel();
            }
        }
        
        protected override void OnCancel()
        {
        }
        
        public void CloseDialog()
        {
            if (!_dialog.CanClose())
            {
                return;
            }
            
            if (_isClosing)
            {
                return;
            }
            
            _isClosing = true;
            
            _dialog.StartClose(_isCanceled);
        }
        
        public void StartClosingDialog()
        {
            if (_closingDialogStarted)
            {
                return;
            }
            
            _closingDialogStarted = true;
            _dialogManager.StartClosingDialog(DialogType);
        }
        
        public void EndClosingDialog()
        {
            _dialogManager.EndClosingDialog(DialogType);
        }
        
        protected override void OnComplete()
        {
        }
        
        protected override void OnKill()
        {
        }
        
        public void OnPressBackground()
        {
            if (_dialog.GetConfig().ShouldCloseOnBackgroundTouch)
            {
                _dialogManager.CloseDialog(DialogType);
            }
        }
        
        public void OnPressEscape()
        {
            if (_dialog.GetConfig().ShouldCloseOnEscape)
            {
                _dialogManager.CancelDialog(DialogType);
            }
        }
        
        public void SetTouchSortingGroup(ZIndex zIndex)
        {
            _dialog.SetTouchSortingGroup(zIndex);
        }
    }
}
