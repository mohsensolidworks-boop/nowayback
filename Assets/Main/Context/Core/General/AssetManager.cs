using Main.Context.Core.Audio;
using Main.Context.Scenes.Common.UI;
using Main.Context.Scenes.Gameplay.General;
using Main.Context.Scenes.Home.General;
using Main.Infrastructure.Context;

namespace Main.Context.Core.General
{
    public sealed class AssetManager : ICoreContextUnit
    {
        private AssetsConfig _assetsConfig => _resourceManager.GetAssetsConfig();
        
        private ResourceManager _resourceManager;
        
        public void Bind()
        {
            _resourceManager = Contexts.Get<ResourceManager>();
        }
        
        public void OnActivateScene()
        {
        }
        
        public CoreAssets GetCoreAssets()
        {
            return _assetsConfig.CoreAssets;
        }
        
        public AudioAssets GetAudioAssets()
        {
            return GetCoreAssets().AudioAssets;
        }
        
        public DialogAssets GetDialogAssets()
        {
            return GetCoreAssets().DialogAssets;
        }
        
        public GameplayAssets GetGameplayAssets()
        {
            return _assetsConfig.GameplayAssets;
        }
        
        public HomeAssets GetHomeAssets()
        {
            return _assetsConfig.HomeAssets;
        }
    }
}
