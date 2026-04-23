using Main.Infrastructure.Context;

namespace Main.Context.Core.General
{
    public sealed class GamePropertiesManager : ICoreContextUnit
    {
        public int LevelsCount => _levelsCount;
        
        private int _levelsCount;
        
        public void Bind()
        {
            var configManager = Contexts.Get<ConfigManager>();
            var levelsConfig = configManager.GetGameplayConfigs().LevelsConfig;
            _levelsCount = levelsConfig.LevelsCount;
        }
        
        public void OnActivateScene()
        {
        }
    }
}
