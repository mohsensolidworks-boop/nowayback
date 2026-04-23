using Main.Context.Core.General;
using Main.Context.Scenes.Common.General;
using Main.Infrastructure.Context;
using UnityEngine;

namespace Main.Context.Scenes.Home.UI
{
    public sealed class HomeUIManager : ISceneContextUnit, IContextBehaviour
    {
        private HomeUI _homeUI;
        
        public void Bind()
        {
            var assetManager = Contexts.Get<AssetManager>();
            var homeUIPrefab = assetManager.GetHomeAssets().HomeUI;
            var homeUIParent = Contexts.Get<CameraManager>().GetCameraTransform();
            _homeUI = Object.Instantiate(homeUIPrefab, homeUIParent);
            _homeUI.Init();
        }
        
        public void Refresh()
        {
            _homeUI.Refresh();
        }
        
        public void ManualUpdate()
        {
            _homeUI.ManualUpdate();
        }
    }
}
