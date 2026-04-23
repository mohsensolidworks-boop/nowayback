using System.Collections.Generic;
using Main.Infrastructure.General;
using UnityEngine;

namespace Main.Context.Scenes.Common.UI.Components
{
    [DisallowMultipleComponent]
    public class UITouchSortingGroup : ManualBehaviour
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

        [SerializeField] private ZIndex _zIndex;
        
        public bool IsAwaken { get; private set; }
        public bool IsChangingParent { get; private set; }
        
        private UITouchSortingGroup _touchSortingGroupParent;
        private bool _isDirty;
        
        public UITouchSortingGroup()
        {
            _isDirty = true;
        }
        
        protected override void Awake()
        {
            IsAwaken = true;
            UpdateTouchSortingGroupParent(true);
            UITouchSortingHelper.SetTouchSortingGroupParentOfChildren(transform, this, new List<ITouchable>(), true);
        }
        
        private void UpdateTouchSortingGroupParent(bool fromAwake)
        {
            if (_isDirty)
            {
                TouchSortingGroupParent = UITouchSortingHelper.GetTouchSortingGroupOnParents(transform.parent, fromAwake);
            }
        }
        
        private void OnBeforeTransformParentChanged()
        {
            if (!IsAwaken)
            {
                return;
            }
            
            IsChangingParent = true;
            
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
            
            IsChangingParent = false;
            
            UpdateTouchSortingGroupParent(false);
        }
    }
}
