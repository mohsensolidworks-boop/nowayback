using Main.Context.Scenes.Gameplay.Level;
using UnityEngine;

namespace Main.Context.Scenes.Gameplay.General
{
    [CreateAssetMenu(fileName = "GameplayConfigs", menuName = "Scriptable Objects/GameplayConfigs")]
    public class GameplayConfigs : ScriptableObject
    {
        [field: SerializeField] public LevelsConfig LevelsConfig { get; private set; }
    }
}
