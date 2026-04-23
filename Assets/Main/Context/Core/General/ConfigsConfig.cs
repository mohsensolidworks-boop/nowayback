using Main.Context.Scenes.Gameplay.General;
using UnityEngine;

namespace Main.Context.Core.General
{
    [CreateAssetMenu(fileName = "ConfigsConfig", menuName = "Scriptable Objects/ConfigsConfig")]
    public class ConfigsConfig : ScriptableObject
    {
        [field: SerializeField] public CoreConfigs CoreConfigs { get; private set; }
        [field: SerializeField] public GameplayConfigs GameplayConfigs { get; private set; }
    }
}
