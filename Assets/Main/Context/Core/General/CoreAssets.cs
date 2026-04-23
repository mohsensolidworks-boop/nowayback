using Main.Context.Core.Audio;
using Main.Context.Scenes.Common.UI;
using UnityEngine;

namespace Main.Context.Core.General
{
    [CreateAssetMenu(fileName = "CoreAssets", menuName = "Scriptable Objects/CoreAssets")]
    public class CoreAssets : ScriptableObject
    {
        [field: SerializeField] public AudioAssets AudioAssets { get; private set; }
        [field: SerializeField] public DialogAssets DialogAssets { get; private set; }
        [field: SerializeField] public DialogBackground DialogBackground { get; private set; }
        [field: SerializeField] public PoolableAudioSourceController PoolableAudioSourceController { get; private set; }
        [field: SerializeField] public Texture2D CursorTexture { get; private set; }
    }
}
