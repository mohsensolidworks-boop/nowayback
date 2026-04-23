using UnityEngine;

namespace Main.Context.Scenes.Common.UI.Components
{
    public interface ITouchable
    {
        public ZIndex ZIndex { get; set; }
        public bool CanTouch { get; }
        public bool IsAwaken { get; set; }
        public UITouchSortingGroup TouchSortingGroupParent { get; set; }
        
        public void TouchDown(Vector2 position);
        public void TouchMove(Vector2 position);
        public void TouchUp(Vector2 position);
        public void CancelTouch(bool isTouchDisabled = false);
    }
    
    public enum ZIndex
    {
        Base0 = 0,
        Base1 = 1,
        Base2 = 2,
        Base3 = 3,
        Base4 = 4,
        Base5 = 5,
        Base6 = 6,
        Base7 = 7,
        Base8 = 8,
        Base9 = 9,
    }
}
