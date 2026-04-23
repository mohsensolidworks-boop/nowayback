using System.Collections.Generic;
using UnityEngine;

namespace Main.Context.Scenes.Common.UI.Components
{
    public static class UITouchSortingHelper
    {
        public static void RemoveTouchSortingGroup(UITouchSortingGroup touchSortingGroup)
        {
            SetTouchSortingGroupParentOfChildren(touchSortingGroup.transform, touchSortingGroup.TouchSortingGroupParent, new List<ITouchable>(), false);
            Object.Destroy(touchSortingGroup);
        }
        
        public static void SetTouchSortingGroupParentOfChildren(Transform transform, UITouchSortingGroup touchSortingGroupParent, List<ITouchable> touchableCache, bool fromAwake)
        {
            transform.GetComponents(touchableCache);
            for (var i = 0; i < touchableCache.Count; i++)
            {
                var touchable = touchableCache[i];
                if (touchable.IsAwaken)
                {
                    touchable.TouchSortingGroupParent = touchSortingGroupParent;
                }
            }
            
            touchableCache.Clear();
            
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var willBeAwakenInThisFrame = fromAwake && child.gameObject.activeInHierarchy;
                if (child.TryGetComponent(out UITouchSortingGroup touchSortingGroup) && (touchSortingGroup.IsAwaken || willBeAwakenInThisFrame))
                {
                    touchSortingGroup.TouchSortingGroupParent = touchSortingGroupParent;
                }
                else
                {
                    SetTouchSortingGroupParentOfChildren(child, touchSortingGroupParent, touchableCache, fromAwake);
                }
            }
        }
        
        public static UITouchSortingGroup GetTouchSortingGroupOnParents(Transform transform, bool fromAwake)
        {
            while (transform != null)
            {
                var willBeAwakenInThisFrame = fromAwake && transform.gameObject.activeInHierarchy;
                if (transform.TryGetComponent(out UITouchSortingGroup touchSortingGroup) && (touchSortingGroup.IsAwaken || willBeAwakenInThisFrame))
                {
                    return touchSortingGroup;
                }
                
                transform = transform.parent;
            }
            
            return null;
        }
    }
}
