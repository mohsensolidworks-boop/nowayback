using Main.Context.Core.Audio;
using Main.Infrastructure.Context;
using UnityEngine;
using UnityEngine.Events;

namespace Main.Context.Scenes.Common.UI.Components
{
    public class UIButton : UITouchComponent
    {
        [SerializeField] private bool _scaleDown = true;
        [SerializeField] private bool _isSilentButton;
        [SerializeField] private bool _immediateCancelOnScroll;
        [SerializeField] private float _overrideScaleFactor;
        [SerializeField] private UnityEvent _onClick;
        [SerializeField] private BoxCollider2D _boxCollider;
        
        private Vector3 _initialScale;
        private bool _scaledDown;
        private bool _didCancel;
        
        public override void TouchDown(Vector2 position)
        {
            ScaleDown();
            _didCancel = false;
        }

        public override void TouchMove(Vector2 position)
        {
            if (!IsInside(position))
            {
                ScaleUp();
            }
        }

        public override void TouchUp(Vector2 position)
        {
            ScaleUp();
            
            if (!_didCancel && IsInside(position))
            {
                InvokeClick();
            }
            
            _didCancel = false;
        }
        
        private void InvokeClick()
        {
            _onClick.Invoke();
            
            if (!_isSilentButton)
            {
                Contexts.Get<AudioManager>().PlaySound(AudioClipType.Button);
            }
        }

        public void AddOnClickListener(UnityAction listener)
        {
            _onClick.AddListener(listener);
        }
        
        public override void CancelTouch(bool isTouchDisabled)
        {
            if (_immediateCancelOnScroll || isTouchDisabled)
            {
                ScaleUp();
            }

            _didCancel = true;
        }

        private void ScaleDown()
        {
            if (!_scaleDown)
            {
                return;
            }

            if (_scaledDown)
            {
                return;
            }

            _scaledDown = true;
            _initialScale = gameObject.transform.localScale;
            PlayScaleDownAnimation();
        }

        protected virtual void PlayScaleDownAnimation()
        {
            transform.localScale *= _overrideScaleFactor > 0 ? _overrideScaleFactor : 0.98f;
        }

        private void ScaleUp()
        {
            if (!_scaleDown || !_scaledDown)
            {
                return;
            }

            _scaledDown = false;
            PlayScaleUpAnimation();
        }

        protected virtual void PlayScaleUpAnimation()
        {
            if (this != null)
            {
                gameObject.transform.localScale = _initialScale;
            }
        }
        
        private bool IsInside(Vector2 position)
        {
            return _boxCollider.bounds.Contains(position);
        }
        
        public void SetFeedbackEnable(bool enable)
        {
            EnableScaling(enable);
        }
        
        private void EnableScaling(bool enable)
        {
            _scaleDown = enable;
        }
        
        protected override void Reset()
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                _boxCollider = GetComponent<BoxCollider2D>();
                base.Reset();
            }
        }
    }
}
