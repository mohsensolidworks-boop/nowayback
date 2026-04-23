using Main.Infrastructure.General;
using UnityEngine;

namespace Main.Context.Scenes.Common.UI.Components
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class UITouchComponent : ManualBehaviour, ITouchable
    {
        public ZIndex ZIndex
        {
            get
            {
                return _zIndex;
            }
            set
            {
                _zIndex = value;
            }
        }
        
        public UITouchSortingGroup TouchSortingGroupParent
        {
            get
            {
                return _touchSortingGroupParent;
            }
            set
            {
                _isDirty = false;
                _touchSortingGroupParent = value;
            }
        }
        
        public bool IsAwaken { get; set; }
        public virtual bool CanTouch => true;
        
        [SerializeField] private ZIndex _zIndex;
        
        private UITouchSortingGroup _touchSortingGroupParent;
        private bool _isDirty;
        
        protected override void Awake()
        {
            _isDirty  = true;
            IsAwaken = true;
            UpdateTouchSortingGroupParent(true);
        }
        
        private void UpdateTouchSortingGroupParent(bool fromAwake)
        {
            if (_isDirty)
            {
                TouchSortingGroupParent = UITouchSortingHelper.GetTouchSortingGroupOnParents(transform, fromAwake);
            }
        }
        
        private void OnBeforeTransformParentChanged()
        {
            if (!IsAwaken)
            {
                return;
            }
            
            var hasSortingGroupParent = TouchSortingGroupParent != null;
            var detachedFromSortingGroupParent = hasSortingGroupParent && !TouchSortingGroupParent.IsChangingParent;
            if (!hasSortingGroupParent || detachedFromSortingGroupParent)
            {
                _isDirty = true;
            }
        }
        
        private void OnTransformParentChanged()
        {
            if (!IsAwaken)
            {
                return;
            }
            
            UpdateTouchSortingGroupParent(false);
        }
        
        public abstract void TouchDown(Vector2 position);

        public abstract void TouchMove(Vector2 position);

        public abstract void TouchUp(Vector2 position);

        public abstract void CancelTouch(bool isTouchDisabled);
        
        public void SetTouchSorting(ZIndex zIndex)
        {
            ZIndex = zIndex;
        }
        
        public void SetTouchSortingGroup(ZIndex zIndex)
        {
            if (TouchSortingGroupParent != null)
            {
                TouchSortingGroupParent.ZIndex = zIndex;
            }
        }
        
        protected virtual void Reset()
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                gameObject.AddComponent<BoxCollider2D>();
                gameObject.tag = "Touchable";
            }
        }
    }
}
