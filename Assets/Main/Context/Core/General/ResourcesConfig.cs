using UnityEngine;

namespace Main.Context.Core.General
{
    [CreateAssetMenu(fileName = "ResourcesConfig", menuName = "Scriptable Objects/ResourceConfig")]
    public class ResourcesConfig : ScriptableObject
    {
        [field: SerializeField] public AssetsConfig AssetsConfig { get; private set; }
        [field: SerializeField] public ConfigsConfig ConfigsConfig { get; private set; }
        [field: SerializeField] public GizmosConfig GizmosConfig { get; private set; }
    }
}
