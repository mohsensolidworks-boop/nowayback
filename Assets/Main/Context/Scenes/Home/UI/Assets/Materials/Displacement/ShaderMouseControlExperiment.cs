using Main.Context.Scenes.Common.General;
using Main.Infrastructure.Context;
using Main.Infrastructure.General;
using UnityEngine;

namespace Main.Context.Scenes.Home.UI.Assets.Materials.Displacement
{
    public class ShaderMouseControlExperiment : ManualBehaviour
    {
        private static int _MAIN_TEX;
        private static int _LIGHT_POS;
        private static int _VECTOR;
        
        [SerializeField] private Renderer _renderer;
        
        private CameraManager _cameraManager;
        private InputManager _inputManager;
        private Material _targetMaterial;
        private Vector2 _previousMousePos2;
        private Vector2 _previousMousePos;
        private Vector2 _tilingOfMaterial;
        private Vector2 _offsetOfMaterial;
        
        public void Init()
        {
            _MAIN_TEX = Shader.PropertyToID("_MainTex");
            _LIGHT_POS = Shader.PropertyToID("_LightPos");
            _VECTOR = Shader.PropertyToID("_Vector");
            
            _cameraManager = Contexts.Get<CameraManager>();
            _inputManager = Contexts.Get<InputManager>();
            var aspectRatio = _cameraManager.AspectRatio;
            var height = _cameraManager.Size * 2f;
            var width = height * aspectRatio;
            _renderer.transform.localScale = new Vector3(width, height, 1f);
            
            var tileCountOnColumn = 13f;
            var tileCountOnRow = tileCountOnColumn * aspectRatio;
            var offsetY = (1f - tileCountOnColumn - Mathf.Floor(tileCountOnColumn)) / 2f;
            var offsetX = (1f - tileCountOnRow - Mathf.Floor(tileCountOnRow)) / 2f;
            _tilingOfMaterial = new Vector2(tileCountOnRow, tileCountOnColumn);
            _offsetOfMaterial = new Vector2(offsetX, offsetY);
            _targetMaterial = _renderer.material;
            _targetMaterial.SetTextureScale(_MAIN_TEX, _tilingOfMaterial);
            _targetMaterial.SetTextureOffset(_MAIN_TEX, _offsetOfMaterial);
        }
        
        protected override void Update()
        {
            var mouseViewportPos = _cameraManager.ScreenToViewportPoint(_inputManager.GetTouchPosition());
            mouseViewportPos.x *= _tilingOfMaterial.x;
            mouseViewportPos.y *= _tilingOfMaterial.y;
            mouseViewportPos += _offsetOfMaterial;
            
            if (_inputManager.GetTouchDown())
            {
                _previousMousePos2 = mouseViewportPos;
                _previousMousePos = mouseViewportPos;
            }
            
            var lightPos = new Vector4(_previousMousePos2.x, _previousMousePos2.y, 0, 0);
            _targetMaterial.SetVector(_LIGHT_POS, lightPos);
            
            var displacement = mouseViewportPos - _previousMousePos;
            var displacementVector = new Vector4(displacement.x, displacement.y, 0, 0) * 3f;
            _targetMaterial.SetVector(_VECTOR, displacementVector);
            
            _previousMousePos2 = Vector2.Lerp(_previousMousePos2, mouseViewportPos, 0.7f);
            _previousMousePos = Vector2.Lerp(_previousMousePos, mouseViewportPos, 0.1f);
        }
    }
}
