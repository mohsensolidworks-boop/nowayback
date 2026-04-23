using Main.Context.Scenes.Gameplay.UI;
using UnityEngine;

namespace Main.Context.Scenes.Gameplay.General
{
    [CreateAssetMenu(fileName = "GameplayAssets", menuName = "Scriptable Objects/GameplayAssets")]
    public class GameplayAssets : ScriptableObject
    {
        [field: SerializeField] public Camera GameplayMainCamera { get; private set; }
        [field: SerializeField] public GameplayUI GameplayUI { get; private set; }
    }
}
