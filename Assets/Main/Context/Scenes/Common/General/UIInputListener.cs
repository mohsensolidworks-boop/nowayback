using System;
using Main.Infrastructure.Context;
using Main.Infrastructure.General;
using Main.Infrastructure.Utils;
using Main.Context.Core.Logger;
using Main.Context.Scenes.Common.UI.Components;
using UnityEngine;

namespace Main.Context.Scenes.Common.General
{
    public sealed class UIInputListener : ISceneContextUnit
    {
        private const int _MAX_COLLIDER_COUNT = 15;
        private const string _BLOCKER_TAG = "Blocker";
        private const string _TOUCHABLE_TAG = "Touchable";
        private const string _SCROLLABLE_TAG = "Scrollable";
        
        public bool IsTouching => _touch.IsSelected || _vertical.IsSelected;
        public bool IsOnUI => _colliderCount > 0;
        
        private CameraManager _cameraManager;
        private EnableCounter _verticalScrollEnabler;
        private RaycastHit2D[] _colliders;
        private int _colliderCount;
        private ITouchable[] _touchables;
        private LayerMask _layerMask;
        private InputManager _inputManager;
        private EnableCounter _inputEnabler;
        private TouchWrapper _touch;
        private ScrollWrapper _vertical;
        
        public void Bind()
        {
            _cameraManager = Contexts.Get<CameraManager>();
            _inputManager = Contexts.Get<InputManager>();
            
            _layerMask = LayerMask.GetMask("UI");
            _inputEnabler = new EnableCounter();
            _touchables = new ITouchable[_MAX_COLLIDER_COUNT];
            _colliders = new RaycastHit2D[_MAX_COLLIDER_COUNT];
            _verticalScrollEnabler = new EnableCounter();
        }
        
        public void ControlInputs()
        {
            var screenPosition = GetTouchPosition();
            _colliderCount = CheckCollision(screenPosition, _colliders);
            
            if (!IsInputEnabled())
            {
                _touch.Cancel(true);
                _touch.RestoreDefaults();
                _vertical.Cancel();
                _vertical.RestoreDefaults();
                return;
            }
            
            if (!IsVerticalScrollEnabled())
            {
                _vertical.Cancel();
            }
            
            if (_inputManager.GetTouchDown())
            {
                _touch.RestoreDefaults();
                _vertical.RestoreDefaults();
                
                TrySelect(_colliders, _colliderCount);
                
                var worldPosition = GetWorldPosition(screenPosition);
                if (_vertical.IsSelected)
                {
                    _vertical.Selected.TouchDown(worldPosition);
                }
                
                if (_touch.IsSelected)
                {
                    _touch.Selected.TouchDown(worldPosition);
                }
            }
            else if (_inputManager.GetTouch())
            {
                var worldPosition = GetWorldPosition(screenPosition);
                if (_vertical.IsSelected)
                {
                    _vertical.Selected.TouchMove(worldPosition);
                    if (_vertical.Selected.IsDragging)
                    {
                        _touch.Cancel(false);
                        _touch.RestoreDefaults();
                    }
                }
                
                if (_touch.IsSelected)
                {
                    _touch.Selected.TouchMove(worldPosition);
                }
            }
            else if (_inputManager.GetTouchUp())
            {
                var worldPosition = GetWorldPosition(screenPosition);
                if (_vertical.IsSelected)
                {
                    _vertical.Selected.TouchUp(worldPosition);
                    _vertical.RestoreDefaults();
                }
                
                if (_touch.IsSelected)
                {
                    _touch.Selected.TouchUp(worldPosition);
                    _touch.RestoreDefaults();
                }
            }
        }
        
        private int CheckCollision(Vector2 screenPosition, RaycastHit2D[] colliders)
        {
            Array.Clear(colliders, 0, colliders.Length);
            var ray = _cameraManager.ScreenToRay(screenPosition);
            return Physics2D.RaycastNonAlloc(ray.origin, ray.direction,  colliders, 30, _layerMask);
        }
        
        private void TrySelect(RaycastHit2D[] colliders, int colliderCount)
        {
            if (colliderCount == 0)
            {
                return;
            }
            else if (colliderCount >= _MAX_COLLIDER_COUNT)
            {
                Log.Warning(this, LogTag.UI, $"All collider array ({colliderCount}) is used, might be more colliders.");
            }
            
            Array.Clear(_touchables, 0, _touchables.Length);
            var touchableCount = 0;
            for (var i = 0; i < colliderCount; i++)
            {
                var collider = colliders[i].collider;
                if (collider != null && collider.TryGetComponent<ITouchable>(out var touchable) && touchable.CanTouch)
                {
                    _touchables[touchableCount] = touchable;
                    touchableCount += 1;
                }
            }
            
            if (touchableCount == 0)
            {
                return;
            }
            else if (touchableCount > 1)
            {
                UITouchSorter.Sort(_touchables, touchableCount);
            }
            
            for (var i = 0; i < touchableCount; i++)
            {
                var touchableGo = ((ManualBehaviour)_touchables[i]).gameObject;
                if (touchableGo.CompareTag(_TOUCHABLE_TAG))
                {
                    if (!_touch.IsSelected)
                    {
                        var touchable = touchableGo.GetComponent<ITouchable>();
                        _touch.SetSelected(touchable);
                    }
                }
                else if (touchableGo.CompareTag(_SCROLLABLE_TAG))
                {
                    if (!_vertical.IsSelected)
                    {
                        var scrollable = touchableGo.GetComponent<IScrollable>();
                        if (scrollable.ScrollDirection == Direction.Vertical)
                        {
                            _vertical.SetSelected(scrollable);
                        }
                    }
                }
                else if (touchableGo.CompareTag(_BLOCKER_TAG))
                {
                }
            }
        }
        
        private Vector2 GetTouchPosition()
        {
            return _inputManager.GetTouchPosition();
        }
        
        private Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            return _cameraManager.ScreenToWorldPoint(screenPosition);
        }
        
        private bool IsVerticalScrollEnabled()
        {
            return _verticalScrollEnabler.IsEnabled();
        }

        public void DisableVerticalScroll()
        {
            _verticalScrollEnabler.Disable();
        }

        public void EnableVerticalScroll()
        {
            _verticalScrollEnabler.Enable();
        }
        
        private bool IsInputEnabled()
        {
            return _inputEnabler.IsEnabled();
        }
        
        public void DisableTouch()
        {
            _inputEnabler.Disable();
        }
        
        public void EnableTouch()
        {
            _inputEnabler.Enable();
        }
        
        private struct TouchWrapper
        {
            public bool IsSelected { get; private set; }
            public ITouchable Selected { get; private set; }
            
            public void SetSelected(ITouchable touchable)
            {
                Selected = touchable;
                IsSelected = Selected != null;
            }
            
            public void Cancel(bool isTouchDisabled)
            {
                if (IsSelected)
                {
                    Selected.CancelTouch(isTouchDisabled);
                }
            }
            
            public void RestoreDefaults()
            {
                SetSelected(null);
            }
        }
        
        private struct ScrollWrapper
        {
            public bool IsSelected { get; private set; }
            public IScrollable Selected  { get; private set; }
            
            public void SetSelected(IScrollable scrollable)
            {
                Selected = scrollable;
                IsSelected = Selected != null;
            }
            
            public void Cancel()
            {
                if (IsSelected)
                {
                    Selected.CancelTouch();
                }
            }
            
            public void RestoreDefaults()
            {
                SetSelected(null);
            }
        }
    }
}
