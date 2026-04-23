using Main.Infrastructure.Context;
using Main.Infrastructure.Utils;
using UnityEngine;

namespace Main.Context.Core.General
{
    public sealed class CursorManager : ICoreContextUnit
    {
        private EnableCounter _cursorEnabler;
        
        public void Bind()
        {
            _cursorEnabler = new EnableCounter();
            
            var assetManager = Contexts.Get<AssetManager>();
            var cursorTexture = assetManager.GetCoreAssets().CursorTexture;
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
        
        public void OnActivateScene()
        {
            _cursorEnabler.RestoreDefaults();
        }
        
        public void DisableCursor()
        {
            var isEnabled = _cursorEnabler.IsEnabled();
            _cursorEnabler.Disable();
            
            if (isEnabled && !_cursorEnabler.IsEnabled())
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        
        public void EnableCursor()
        {
            var isEnabled = _cursorEnabler.IsEnabled();
            _cursorEnabler.Enable();
            
            if (!isEnabled && _cursorEnabler.IsEnabled())
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        
        public bool IsCursorDisabled()
        {
            return Cursor.lockState == CursorLockMode.Locked;
        }
    }
}
