using System.Collections.Generic;
using Main.Context.Core.Logger;
using Main.Context.Scenes.Common.General;
using Main.Context.Scenes.Common.UI.Components;
using Main.Infrastructure.Context;
using UnityEngine;

namespace Main.Context.Scenes.Common.UI
{
    public class DialogBackground : UIPanel
    {
        [SerializeField] private DialogBackgroundDark _backgroundPrefab;
        [SerializeField] private BoxCollider2D _touchCollider;
        [SerializeField] private UIPanel _blocker;
        [SerializeField] private BoxCollider2D _blockerCollider;
        
        private Dictionary<DialogType, DialogBackgroundDark> _dialogBackgrounds;
        private List<DialogBackgroundDark> _backgrounds;
        private CameraManager _cameraManager;
        private DialogManager _dialogManager;
        
        public void Init(Transform dialogParent)
        {
            _dialogBackgrounds = new Dictionary<DialogType, DialogBackgroundDark>();
            _backgrounds = new List<DialogBackgroundDark>();
            _dialogManager = Contexts.Get<DialogManager>();
            _cameraManager = Contexts.Get<CameraManager>();
            
            var t = transform;
            t.SetParent(dialogParent);
            t.localPosition = new Vector3(0, 0, 10);
            t.localRotation = Quaternion.identity;
            
            var size = GetSize();
            _touchCollider.size = size;
            _blockerCollider.size = size;
            
            gameObject.SetActive(false);
        }
        
        public void Show(DialogType dialogType, SortingData sortingData, bool getFirst)
        {
            if (!_dialogBackgrounds.ContainsKey(dialogType))
            {
                _dialogBackgrounds.Add(dialogType, GetBackgroundDark(getFirst));
            }
            else
            {
                Log.Error(this, LogTag.Dialog, $"There is a background assigned to this dialog: {dialogType}");
            }
            
            _dialogBackgrounds[dialogType].Show(sortingData);
            
            gameObject.SetActive(true);
        }
        
        public void Hide(DialogType dialogType)
        {
            if (!_dialogBackgrounds.ContainsKey(dialogType))
            {
                OnHideComplete();
                Log.Error(this, LogTag.Dialog, $"There is no background assigned to this dialog: {dialogType}");
                return;
            }

            _dialogBackgrounds[dialogType].Hide();
            
            _dialogBackgrounds.Remove(dialogType);
            OnHideComplete();
        }
        
        private void OnHideComplete()
        {
            for (var i = 0; i < _backgrounds.Count; i++)
            {
                if (_backgrounds[i].gameObject.activeSelf)
                {
                    return;
                }
            }
            
            gameObject.SetActive(false);
        }
        
        private DialogBackgroundDark GetBackgroundDark(bool getFirst)
        {
            for (var i = 0; i < _backgrounds.Count; i++)
            {
                if (getFirst || !_backgrounds[i].IsAllocated)
                {
                    return _backgrounds[i];
                }
            }
            
            var size = GetSize();
            var background = Instantiate(_backgroundPrefab, transform);
            background.Init(size);
            _backgrounds.Add(background);
            
            return background;
        }
        
        public override void TouchUp(Vector2 position)
        {
            _dialogManager.PressBackground();
        }
        
        private Vector2 GetSize()
        {
            var height = _cameraManager.Size * 2;
            var width = height * _cameraManager.AspectRatio;
            return new Vector2(width, height);
        }
        
        public void SetBlockerTouchSorting(ZIndex zIndex)
        {
            _blocker.SetTouchSorting(zIndex);
        }
    }
}
