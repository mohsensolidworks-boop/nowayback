using System;
using System.Collections.Generic;
using System.Linq;
using Main.Context.Core.General;
using Main.Context.Core.Logger;
using Main.Context.Scenes.Common.General;
using Main.Context.Scenes.Common.UI.Components;
using Main.Context.Scenes.Gameplay.UI.PauseDialog;
using Main.Context.Scenes.Home.UI.AdminDialog;
using Main.Context.Scenes.Home.UI.OptionsDialog;
using Main.Infrastructure.Context;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Main.Context.Scenes.Common.UI
{
    public sealed class DialogManager : ISceneContextUnit
    {
        private const ZIndex _BEHIND_DIALOG_Z_INDEX = ZIndex.Base0;
        private const ZIndex _DIALOG_BACKGROUND_BLOCKER_Z_INDEX = ZIndex.Base1;
        private const ZIndex _DIALOG_BACKGROUND_Z_INDEX = ZIndex.Base2;
        private const ZIndex _TOP_DIALOG_Z_INDEX = ZIndex.Base3;
        
        private bool _hasActiveDialog => _dialogActions.Count > 0;
        private bool _hasMultipleDialogs => _dialogActions.Count > 1;
        
        private AssetManager _assetManager;
        private FlowManager _flowManager;
        private DialogBackground _dialogBackground;
        private SortedDictionary<DialogType, DialogAction> _dialogActions;
        private int _sortingOrder;
        
        public void Bind()
        {
            _assetManager = Contexts.Get<AssetManager>();
            _flowManager = Contexts.Get<FlowManager>();
            
            _dialogActions = new SortedDictionary<DialogType, DialogAction>();
            var dialogBackgroundPrefab = _assetManager.GetCoreAssets().DialogBackground;
            _dialogBackground = Object.Instantiate(dialogBackgroundPrefab);
            _dialogBackground.Init(GetDialogParent());
        }
        
        public bool IsDialogOpen(DialogType dialogType)
        {
            return _dialogActions.ContainsKey(dialogType);
        }
        
        public void OpenDialog(DialogType dialogType, Action onComplete = null)
        {
            DialogAction dialogAction = dialogType switch
            {
                DialogType.AdminDialog => new ShowAdminDialogAction(dialogType, onComplete),
                DialogType.OptionsDialog => new ShowOptionsDialogAction(dialogType, onComplete),
                DialogType.PauseDialog => new ShowPauseDialogAction(dialogType, onComplete),
                _ => throw new ArgumentOutOfRangeException(nameof(dialogType), dialogType, null)
            };
            
            _flowManager.Push(dialogAction);
        }
        
        public void ShowDialog(DialogAction dialogAction)
        {
            if (_dialogActions.ContainsValue(dialogAction))
            {
                Log.Error(this, LogTag.Dialog, $"DialogManager already has dialog: {dialogAction.DialogType}");
                return;
            }
            
            _dialogActions.Add(dialogAction.DialogType, dialogAction);
            
            _sortingOrder += 1;
            var dialogSorting = UISorting.GetDialogSorting().GetSortingWithOffset(GetSpriteSortingOffset(_sortingOrder));
            var backgroundSorting = UISorting.GetDialogSorting().GetSortingWithOffset(GetSpriteSortingOffset(_sortingOrder) - 1);
            _dialogBackground.Show(dialogAction.DialogType, backgroundSorting, !_hasMultipleDialogs);
            
            dialogAction.ShowDialog(dialogSorting, GetDialogParent());
            ResetTouchSortings();
        }
        
        public void CancelDialog(DialogType dialogType)
        {
            if (!_dialogActions.ContainsKey(dialogType))
            {
                return;
            }
            
            _dialogActions[dialogType].CancelDialog();
        }
        
        public void CloseDialog(DialogType dialogType)
        {
            if (!_dialogActions.ContainsKey(dialogType))
            {
                return;
            }
            
            _dialogActions[dialogType].CloseDialog();
        }
        
        public void StartClosingDialog(DialogType dialogType)
        {
            if (!_dialogActions.ContainsKey(dialogType))
            {
                Log.Error(this, LogTag.Dialog, $"DialogManager does not contain dialog: {dialogType}");
                return;
            }
            
            _dialogBackground.Hide(dialogType);
        }
        
        public void EndClosingDialog(DialogType dialogType)
        {
            if (dialogType == GetDialogActionTypeOnTop())
            {
                _sortingOrder -= 1;
            }
            
            _dialogActions.Remove(dialogType);
            
            if (_hasActiveDialog)
            {
                ResetTouchSortings();
            }
            else
            {
                _sortingOrder = 0;
            }
        }
        
        private void ResetTouchSortings()
        {
            foreach (var (_, dialogActions) in _dialogActions)
            {
                dialogActions.SetTouchSortingGroup(_BEHIND_DIALOG_Z_INDEX);
            }
            
            _dialogBackground.SetBlockerTouchSorting(_DIALOG_BACKGROUND_BLOCKER_Z_INDEX);
            _dialogBackground.SetTouchSorting(_DIALOG_BACKGROUND_Z_INDEX);
            GetDialogActionOnTop().SetTouchSortingGroup(_TOP_DIALOG_Z_INDEX);
        }
        
        private int GetSpriteSortingOffset(int order)
        {
            return (order - 1) * 4;
        }
        
        public void PressBackground()
        {
            if (!_hasActiveDialog)
            {
                return;
            }
            
            GetDialogActionOnTop().OnPressBackground();
        }
        
        public void PressBack()
        {
            if (!_hasActiveDialog)
            {
                return;
            }
            
            GetDialogActionOnTop().OnPressEscape();
        }
        
        private DialogType GetDialogActionTypeOnTop()
        {
            return _dialogActions.Last().Key;
        }
        
        private DialogAction GetDialogActionOnTop()
        {
            return _dialogActions[GetDialogActionTypeOnTop()];
        }
        
        private Transform GetDialogParent()
        {
            return Contexts.Get<CameraManager>().GetCameraTransform();
        }
    }
}
