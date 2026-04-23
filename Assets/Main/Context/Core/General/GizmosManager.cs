using Main.Infrastructure.Context;

namespace Main.Context.Core.General
{
    public sealed class GizmosManager : ICoreContextUnit
    {
        private GizmosConfig _gizmosConfig => _resourceManager.GetGizmosConfig();
        
        private ResourceManager _resourceManager;
        
        public void Bind()
        {
            _resourceManager = Contexts.Get<ResourceManager>();
        }
        
        public void OnActivateScene()
        {
        }
    }
}
