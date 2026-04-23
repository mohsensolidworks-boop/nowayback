using Main.Context.Scenes.Home.UI;
using UnityEngine;

namespace Main.Context.Scenes.Home.General
{
    [CreateAssetMenu(fileName = "HomeAssets", menuName = "Scriptable Objects/HomeAssets")]
    public class HomeAssets : ScriptableObject
    {
        [field: SerializeField] public Camera HomeMainCamera { get; private set; }
        [field: SerializeField] public HomeUI HomeUI { get; private set; }
    }
}
