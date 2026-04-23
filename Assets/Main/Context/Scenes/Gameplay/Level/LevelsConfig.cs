using UnityEngine;

namespace Main.Context.Scenes.Gameplay.Level
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "Scriptable Objects/LevelsConfig")]
    public class LevelsConfig : ScriptableObject
    {
        [field: SerializeField] public int LevelsCount { get; private set; }
    }
}
