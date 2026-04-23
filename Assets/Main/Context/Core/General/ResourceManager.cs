using Main.Infrastructure.Context;
using UnityEngine;

namespace Main.Context.Core.General
{
    public sealed class ResourceManager : ICoreContextUnit
    {
        private ResourcesConfig _resourceConfig;
        
        public void Bind()
        {
            _resourceConfig = Load<ResourcesConfig>("ResourcesConfig");
        }
        
        public void OnActivateScene()
        {
        }
        
        public AssetsConfig GetAssetsConfig()
        {
            return _resourceConfig.AssetsConfig;
        }
        
        public ConfigsConfig GetConfigsConfig()
        {
            return _resourceConfig.ConfigsConfig;
        }
        
        public GizmosConfig GetGizmosConfig()
        {
            return _resourceConfig.GizmosConfig;
        }
        
        public static T Load<T>(string path) where T : Object
        {
            return (T)Resources.Load(path, typeof(T));
        }
    }
}
