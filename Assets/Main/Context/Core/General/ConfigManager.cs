using Main.Context.Scenes.Gameplay.General;
using Main.Infrastructure.Context;

namespace Main.Context.Core.General
{
    public sealed class ConfigManager : ICoreContextUnit
    {
        private ConfigsConfig _configsConfig => _resourceManager.GetConfigsConfig();
        
        private ResourceManager _resourceManager;
        
        public void Bind()
        {
            _resourceManager = Contexts.Get<ResourceManager>();
        }
        
        public void OnActivateScene()
        {
        }
        
        public CoreConfigs GetCoreConfigs()
        {
            return _configsConfig.CoreConfigs;
        }
        
        public GameplayConfigs GetGameplayConfigs()
        {
            return _configsConfig.GameplayConfigs;
        }
    }
}
