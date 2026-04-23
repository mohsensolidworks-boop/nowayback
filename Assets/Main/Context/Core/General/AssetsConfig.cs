using Main.Context.Scenes.Gameplay.General;
using Main.Context.Scenes.Home.General;
using UnityEngine;

namespace Main.Context.Core.General
{
    [CreateAssetMenu(fileName = "AssetsConfig", menuName = "Scriptable Objects/AssetsConfig")]
    public class AssetsConfig : ScriptableObject
    {
        [field: SerializeField] public CoreAssets CoreAssets { get; private set; }
        [field: SerializeField] public HomeAssets HomeAssets { get; private set; }
        [field: SerializeField] public GameplayAssets GameplayAssets { get; private set; }
    }
}
