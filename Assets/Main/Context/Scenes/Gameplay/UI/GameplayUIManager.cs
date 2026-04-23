using Main.Context.Core.General;
using Main.Context.Scenes.Common.General;
using Main.Infrastructure.Context;
using UnityEngine;

namespace Main.Context.Scenes.Gameplay.UI
{
    public sealed class GameplayUIManager : ISceneContextUnit
    {
        private GameplayUI _gameplayUI;
        
        public void Bind()
        {
            var assetManager = Contexts.Get<AssetManager>();
            var gameplayUIPrefab = assetManager.GetGameplayAssets().GameplayUI;
            var gameplayUIParent = Contexts.Get<CameraManager>().GetCameraTransform();
            _gameplayUI = Object.Instantiate(gameplayUIPrefab, gameplayUIParent);
            _gameplayUI.Init();
        }
    }
}
