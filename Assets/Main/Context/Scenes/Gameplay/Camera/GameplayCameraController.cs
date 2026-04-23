using Main.Context.Scenes.Common.General;
using Main.Infrastructure.General;
using Main.Infrastructure.Context;
using UnityEngine;

namespace Main.Context.Scenes.Gameplay.Cam
{
    public class GameplayCameraController : ACameraController
    {
        private const float _SCROLL_WHEEL_SPEED = 5f;
        private const float _DRAG_SPEED = 2f;
        private const float _SMOOTHING = 10f;
        
        private InputManager _inputManager;
        private float _minX;
        private float _maxX;
        private float _targetX;
        
        protected override void Init()
        {
            _inputManager = Contexts.Get<InputManager>();
        }
        
        public void SetLimits(float areaWidth)
        {
            var areaHalf = areaWidth / 2f;
            // Assuming your SafeWidth/Ratio logic calculates the horizontal viewport extent
            var cameraHalf = SafeWidth / SafeHeightRatio / 100f / 2f;
            var maxX = Mathf.Clamp(areaHalf - cameraHalf, 0f, areaWidth);
            _minX = -maxX;
            _maxX = maxX;
            
            // Initialize targetX to current position to prevent snapping on start
            _targetX = MainCamera.transform.position.x;
        }
        
        public override void Iterate()
        {
            return;
            ControlInputs();
        }
        
        public void ControlInputs()
        {
            // 1. Mouse Wheel Scroll
            var mouseScrollDeltaY = _inputManager.GetMouseScrollDelta().y;
            if (mouseScrollDeltaY != 0)
            {
                // Inverting the delta is common for "natural" feeling scroll
                _targetX -= mouseScrollDeltaY * _SCROLL_WHEEL_SPEED;
            }
            
            // 2. Click and Drag Scroll (Middle or Left mouse)
            if (_inputManager.GetTouch() || _inputManager.GetThirdTouch())
            {
                var mouseX = _inputManager.GetHorizontalAxis();
                _targetX -= mouseX * _DRAG_SPEED;
            }

            // Keep target within bounds
            _targetX = Mathf.Clamp(_targetX, _minX, _maxX);
            
            var cameraPosition = MainCamera.transform.position;
            
            // Using Lerp for that "elastic" smooth feeling
            var newX = Mathf.Lerp(cameraPosition.x, _targetX, Time.deltaTime * _SMOOTHING);
            
            cameraPosition.x = newX;
            MainCamera.transform.position = cameraPosition;
        }
    }
}
