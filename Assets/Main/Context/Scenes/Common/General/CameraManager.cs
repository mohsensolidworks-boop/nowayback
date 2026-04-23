using Main.Context.Core.General;
using Main.Context.Scenes.Gameplay.Cam;
using Main.Context.Scenes.Home.Cam;
using Main.Infrastructure.Context;
using Main.Infrastructure.General;
using UnityEngine;

namespace Main.Context.Scenes.Common.General
{
    public sealed class CameraManager : ISceneContextUnit, IContextBehaviour
    {
        public float Height => _cameraController.SafeHeight;
        public float Width => _cameraController.SafeWidth;
        public float AspectRatio => _cameraController.AspectRatio;
        public float Size => _cameraController.Size;
        public float SafeHeightRatio => _cameraController.SafeHeightRatio;
        
        private ACameraController _cameraController;
        private Camera _mainCamera;
        
        public void Bind()
        {
            var isHomeScene = Contexts.Get<SceneChangeManager>().IsHomeScene;
            var assetManager = Contexts.Get<AssetManager>();
            var cameraPrefab = isHomeScene ? assetManager.GetHomeAssets().HomeMainCamera : assetManager.GetGameplayAssets().GameplayMainCamera;
            ACameraController cameraController = isHomeScene ? new HomeCameraController() : new GameplayCameraController();
            
            _mainCamera = isHomeScene ? Object.Instantiate(cameraPrefab) : Object.FindObjectOfType<Camera>();
            _cameraController = cameraController;
            _cameraController.Prepare(_mainCamera);
        }
        
        public void ManualUpdate()
        {
            _cameraController.Iterate();
        }
        
        public void ManualLateUpdate()
        {
        }
        
        public Transform GetCameraTransform()
        {
            return _cameraController.MainCamera.transform;
        }

        public Vector2 ScreenToWorldPoint(Vector2 position)
        {
            return _cameraController.MainCamera.ScreenToWorldPoint(position);
        }
        
        public Ray ScreenToRay(Vector2 position)
        {
            return _cameraController.MainCamera.ScreenPointToRay(position);
        }
        
        public Vector2 ScreenToViewportPoint(Vector3 position)
        {
            return _cameraController.MainCamera.ScreenToViewportPoint(position);
        }
        
        public ACameraController GetCameraController()
        {
            return _cameraController;
        }
    }
}
