using UnityEngine;

namespace Main.Infrastructure.General
{
    public abstract class ACameraController
    {
        public Camera MainCamera { get; private set; }
        public float SafeWidth { get; private set; }
        public float SafeHeight { get; private set; }
        public float AspectRatio { get; private set; }
        public float SafeHeightRatio { get; private set;}
        public float Size { get; private set; }
        
        public void Prepare(Camera mainCamera)
        {
            var safeArea = Screen.safeArea;
            MainCamera = mainCamera;
            SafeWidth = safeArea.width;
            SafeHeight = safeArea.height;
            Size = MainCamera.orthographicSize;
            AspectRatio = SafeWidth / SafeHeight;
            SafeHeightRatio = SafeHeight / (Size * 200);
            
            Init();
        }
        
        protected virtual void Init()
        {
        }
        
        public virtual void Iterate()
        {
        }
    }
}
