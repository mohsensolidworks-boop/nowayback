using System.Collections.Generic;
using Main.Context.Core.Logger;

namespace Main.Context.Scenes.Common.UI.Components
{
    public static class UITouchSorter
    {
        private static readonly Stack<ZIndex> _SORTING_GROUP_PARENTS1;
        private static readonly Stack<ZIndex> _SORTING_GROUP_PARENTS2;

        static UITouchSorter()
        {
            _SORTING_GROUP_PARENTS1 = new Stack<ZIndex>();
            _SORTING_GROUP_PARENTS2 = new Stack<ZIndex>();
        }
        
        public static void Sort(ITouchable[] touchables, int touchablesCount)
        {
            for (var i = 0; i < touchablesCount - 1; i++)
            {
                for (var j = i + 1; j > 0; j--)
                {
                    var touchable1 = touchables[j - 1];
                    var touchable2 = touchables[j];
                    if (ShouldSwap(touchable1, touchable2))
                    {
                        touchables[j - 1] = touchable2;
                        touchables[j] = touchable1;
                    }
                }
            }
        }
        
        private static bool ShouldSwap(ITouchable touchable1, ITouchable touchable2)
        {
            if (touchable2 == null)
            {
                return false;
            }
            
            if (touchable1 == null)
            {
                return true;
            }
            
            GetSortingGroupParents(touchable1, _SORTING_GROUP_PARENTS1);
            GetSortingGroupParents(touchable2, _SORTING_GROUP_PARENTS2);
            
            var zIndex1 = _SORTING_GROUP_PARENTS1.Pop();
            var zIndex2 = _SORTING_GROUP_PARENTS2.Pop();
            while (_SORTING_GROUP_PARENTS1.Count > 0 && _SORTING_GROUP_PARENTS2.Count > 0)
            {
                if (zIndex1 != zIndex2)
                {
                    break;
                }
                
                zIndex1 = _SORTING_GROUP_PARENTS1.Pop();
                zIndex2 = _SORTING_GROUP_PARENTS2.Pop();
            }
            
            _SORTING_GROUP_PARENTS1.Clear();
            _SORTING_GROUP_PARENTS2.Clear();
            
            return zIndex1 < zIndex2;
        }
        
        private static void GetSortingGroupParents(ITouchable touchable, Stack<ZIndex> sortingGroupParents)
        {
            sortingGroupParents.Push(touchable.ZIndex);
            
            var parentCount = 0;
            var sortingGroup = touchable.TouchSortingGroupParent;
            while (sortingGroup != null)
            {
                sortingGroupParents.Push(sortingGroup.ZIndex);
                sortingGroup = sortingGroup.TouchSortingGroupParent;
                
                parentCount += 1;
                if (parentCount > 1000)
                {
                    Log.Error(touchable, LogTag.UI, $"There may be parent loop: {touchable} {sortingGroup}");
                    break;
                }
            }
        }
    }
}
