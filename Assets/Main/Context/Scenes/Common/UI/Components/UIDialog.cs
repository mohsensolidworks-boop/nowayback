using System.Collections;
using Main.Infrastructure.Context;
using Main.Infrastructure.Utils.Extensions;
using UnityEngine;
using UnityEngine.Rendering;

namespace Main.Context.Scenes.Common.UI.Components
{
    [RequireComponent(typeof(SortingGroup))]
    [RequireComponent(typeof(UITouchSortingGroup))]
    public class UIDialog : UIPanel
    {
        private DialogAction _dialogAction;
        
        private bool _isKilled;
        
        #region ShowFlow
        
        public void StartShow(DialogAction dialogAction, SortingData dialogSorting, Transform dialogParent)
        {
            _dialogAction = dialogAction;
            var dialogTransform = transform;
            dialogTransform.SetParent(dialogParent);
            dialogTransform.localPosition = new Vector3(0, 0, 10);
            dialogTransform.localRotation = Quaternion.identity;
            
            SetSorting(dialogSorting);
            
            OnShow();
        }
        
        protected virtual void OnShow()
        {
        }
        
        #endregion
        
        #region CloseFlow
        
        protected void Close()
        {
            Contexts.Get<DialogManager>().CloseDialog(_dialogAction.DialogType);
        }
        
        public virtual bool CanClose()
        {
            return true;
        }
        
        public void StartClose(bool isCanceled)
        {
            _dialogAction.StartClosingDialog();
            Contexts.ManualStartCoroutine(CloseFlow(isCanceled));
        }
        
        private IEnumerator CloseFlow(bool isCanceled)
        {
            if (GetConfig().ShouldPlayCloseAnimation)
            {
                yield return CloseAnimation();
            }
            
            if (isCanceled)
            {
                yield break;
            }
            
            _dialogAction.EndClosingDialog();
            OnClose();
            _dialogAction.Complete();
            Kill();
        }
        
        protected virtual IEnumerator CloseAnimation()
        {
            yield break;
        }
        
        protected virtual void OnClose()
        {
        }
        
        #endregion
        
        #region CancelFlow
        
        public void Cancel()
        {
            Contexts.Get<DialogManager>().CancelDialog(_dialogAction.DialogType);
        }
        
        public void StartCancel()
        {
            _dialogAction.StartClosingDialog();
            CancelFlow();
        }
        
        private void CancelFlow()
        {
            _dialogAction.EndClosingDialog(); 
            OnCancel();
            _dialogAction.Cancel();
            Kill();
        }
        
        protected virtual void OnCancel()
        {
        }
        
        #endregion
        
        #region KillFlow
        
        private void Kill()
        {
            if (_isKilled)
            {
                return;
            }
            
            _isKilled = true;
            OnKill();
            Destroy(gameObject);
        }
        
        protected virtual void OnKill()
        {
        }
        
        #endregion

        protected SortingGroup GetSortingGroup()
        {
            return GetComponent<SortingGroup>();
        }

        private void SetSorting(SortingData dialogSorting)
        {
            GetSortingGroup().SetSorting(dialogSorting);
        }
        
        protected override void Reset() 
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                base.Reset();
                SetSorting(UISorting.GetDialogSorting());
                TouchSortingGroupParent = gameObject.AddComponent<UITouchSortingGroup>();
                SetTouchSortingGroup(ZIndex.Base0);
            }
        }
        
        public virtual DialogConfig GetConfig()
        {
            return new DialogConfig
            {
                ShouldCloseOnEscape = true,
                ShouldCloseOnBackgroundTouch = true,
                ShouldPlayCloseAnimation = false,
            };
        }
    }
    
    public struct DialogConfig
    {
        public bool ShouldCloseOnEscape;
        public bool ShouldCloseOnBackgroundTouch;
        public bool ShouldPlayCloseAnimation;
    }
}
