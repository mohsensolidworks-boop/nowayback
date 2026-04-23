using System;
using UnityEngine;

namespace Main.Context.Core.Audio
{
    [CreateAssetMenu(fileName = "AudioAssets", menuName = "Scriptable Objects/AudioAssets")]
    public class AudioAssets : ScriptableObject
    {
        [field: SerializeField] public AudioClip Button { get; private set; }
        [field: SerializeField] public AudioClip FootstepWood1 { get; private set; }
        [field: SerializeField] public AudioClip FootstepWood2 { get; private set; }
        [field: SerializeField] public AudioClip FootstepWood3 { get; private set; }
        [field: SerializeField] public AudioClip AmbienceNature { get; private set; }
        [field: SerializeField] public AudioClip HomeBackgroundAmbience { get; private set; }
        [field: SerializeField] public AudioClip HomeBackgroundAmbienceAddition1 { get; private set; }
        [field: SerializeField] public AudioClip HomeBackgroundAmbienceAddition2 { get; private set; }
        [field: SerializeField] public AudioClip HomeBackgroundMusic1 { get; private set; }
        [field: SerializeField] public AudioClip AreaBackgroundAmbience { get; private set; }
        [field: SerializeField] public AudioClip CursorTap1 { get; private set; }
        [field: SerializeField] public AudioClip CursorTap2 { get; private set; }
        [field: SerializeField] public AudioClip CursorTap3 { get; private set; }
        [field: SerializeField] public AudioClip CursorTap4 { get; private set; }
        [field: SerializeField] public AudioClip CatMeow1 { get; private set; }
        [field: SerializeField] public AudioClip CatMeow2 { get; private set; }
        [field: SerializeField] public AudioClip CatMeow3 { get; private set; }
        [field: SerializeField] public AudioClip CatMeow4 { get; private set; }
        [field: SerializeField] public AudioClip CatMeow5 { get; private set; }
        [field: SerializeField] public AudioClip CatMeow6 { get; private set; }
        [field: SerializeField] public AudioClip CatMeow7 { get; private set; }
        [field: SerializeField] public AudioClip CatMeow8 { get; private set; }
        [field: SerializeField] public AudioClip CatMeow9 { get; private set; }
        [field: SerializeField] public AudioClip CatMeow10 { get; private set; }
        [field: SerializeField] public AudioClip CatMeow11 { get; private set; }
        [field: SerializeField] public AudioClip CatMeow12 { get; private set; }
        [field: SerializeField] public AudioClip CatMeow13 { get; private set; }
        
        public AudioClip GetAudioClip(AudioClipType type)
        {
            return type switch
            {
                AudioClipType.None => null,
                AudioClipType.Button => Button,
                AudioClipType.FootstepWood1 => FootstepWood1,
                AudioClipType.FootstepWood2 => FootstepWood2,
                AudioClipType.FootstepWood3 => FootstepWood3,
                AudioClipType.AmbienceNature => AmbienceNature,
                AudioClipType.HomeBackgroundAmbience => HomeBackgroundAmbience,
                AudioClipType.HomeBackgroundAmbienceAddition1 => HomeBackgroundAmbienceAddition1,
                AudioClipType.HomeBackgroundAmbienceAddition2 => HomeBackgroundAmbienceAddition2,
                AudioClipType.HomeBackgroundMusic1 => HomeBackgroundMusic1,
                AudioClipType.AreaAmbienceBackground => AreaBackgroundAmbience,
                AudioClipType.CursorTap1 => CursorTap1,
                AudioClipType.CursorTap2 => CursorTap2,
                AudioClipType.CursorTap3 => CursorTap3,
                AudioClipType.CursorTap4 => CursorTap4,
                AudioClipType.CatMeow1 => CatMeow1,
                AudioClipType.CatMeow2 => CatMeow2,
                AudioClipType.CatMeow3 => CatMeow3,
                AudioClipType.CatMeow4 => CatMeow4,
                AudioClipType.CatMeow5 => CatMeow5,
                AudioClipType.CatMeow6 => CatMeow6,
                AudioClipType.CatMeow7 => CatMeow7,
                AudioClipType.CatMeow8 => CatMeow8,
                AudioClipType.CatMeow9 => CatMeow9,
                AudioClipType.CatMeow10 => CatMeow10,
                AudioClipType.CatMeow11 => CatMeow11,
                AudioClipType.CatMeow12 => CatMeow12,
                AudioClipType.CatMeow13 => CatMeow13,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
