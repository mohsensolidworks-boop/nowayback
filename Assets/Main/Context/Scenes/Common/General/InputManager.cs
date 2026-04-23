using Main.Infrastructure.Context;
using UnityEngine;

namespace Main.Context.Scenes.Common.General
{
    public sealed class InputManager : ISceneContextUnit
    {
        public void Bind()
        {
        }
        
        public Vector2 GetTouchPosition()
        {
            return Input.mousePosition;
        }
        
        public bool GetTouchDown()
        {
            return Input.GetMouseButtonDown(0);
        }
        
        public bool GetTouch()
        {
            return Input.GetMouseButton(0);
        }
        
        public bool GetTouchUp()
        {
            return Input.GetMouseButtonUp(0);
        }
        
        public bool GetThirdTouch()
        {
            return Input.GetMouseButton(2);
        }
        
        public bool GetKeyUp(KeyCode keyCode)
        {
            return Input.GetKeyUp(keyCode);
        }
        
        public float GetHorizontalAxis()
        {
            return Input.GetAxis("Mouse X");
        }
        
        public Vector2 GetMouseScrollDelta()
        {
            return Input.mouseScrollDelta;
        }
    }
}
