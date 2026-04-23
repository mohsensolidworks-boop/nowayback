using System.Collections;
using System.Collections.Generic;
using Main.Context.Core.General;
using Main.Context.Core.Logger;
using Main.Infrastructure.Context;
using Main.Infrastructure.General;
using UnityEngine;

namespace Main.Context.Core.Audio
{
    public sealed class AudioManager : ICoreContextUnit
    {
        private const int _MAX_SAME_TYPE_SOUNDS_PLAYED_PARALLEL = 5;
        
        private Dictionary<long, PoolableAudioSourceController> _keyAudioSource;
        private Dictionary<AudioClipType, Queue<PoolableAudioSourceController>> _loopedAudioSources;
        private Dictionary<AudioClipType, int> _loopedAudioCounter;
        private GameObjectPool _audioSourcePool;
        private int _stoppableAudioKey;
        private AssetManager _assetManager;
        
        public void Bind()
        {
            _assetManager = Contexts.Get<AssetManager>();
            
            _keyAudioSource = new Dictionary<long, PoolableAudioSourceController>();
            _loopedAudioSources = new Dictionary<AudioClipType, Queue<PoolableAudioSourceController>>();
            _loopedAudioCounter = new Dictionary<AudioClipType, int>();
            _audioSourcePool = new GameObjectPool(typeof(AudioSourcePoolId));
            var audioSourcePrefab = _assetManager.GetCoreAssets().PoolableAudioSourceController;
            _audioSourcePool.CreatePool(audioSourcePrefab, 6);
        }

        public void OnActivateScene()
        {
        }

        public void PlaySound(AudioClipType audioClipType)
        {
            var audioClip = GetAudioClip(audioClipType);
            if (audioClip == null)
            {
                Log.Error(this, LogTag.Audio, $"Clip is null: {audioClipType}");
                return;
            }

            var audioSource = GetAudioSource();
            audioSource.PlayOneShot(audioClip);
            
            _keyAudioSource[_stoppableAudioKey] = audioSource;
            Contexts.ManualStartCoroutine(ReleasePoolableAudioSourceWhenPlayIsOver(_stoppableAudioKey));
            
            _stoppableAudioKey += 1;
        }
        
        private IEnumerator ReleasePoolableAudioSourceWhenPlayIsOver(long key)
        {
            if (!_keyAudioSource.TryGetValue(key, out var poolableAudioSource))
            {
                yield break;
            }
            
            while (poolableAudioSource.IsPlaying())
            {
                yield return null;
            }
            
            if (_keyAudioSource.TryGetValue(key, out var audioSource))
            {
                if (audioSource != null)
                {
                    audioSource.Stop();
                    _audioSourcePool.Recycle(audioSource);
                }
                
                _keyAudioSource.Remove(key);
            }
        }
        
        public void PlaySoundInLoop(AudioClipType audioClipType)
        {
            _loopedAudioCounter.TryAdd(audioClipType, 0);
            if (_loopedAudioCounter[audioClipType] > _MAX_SAME_TYPE_SOUNDS_PLAYED_PARALLEL)
            {
                return;
            }
            
            var audioClip = GetAudioClip(audioClipType);
            if (audioClip == null)
            {
                Log.Error(this, LogTag.Audio, $"Clip is null: {audioClipType}");
                return;
            }

            var audioSource = GetAudioSource();
            audioSource.PlayLoop(audioClip);
            
            _loopedAudioSources.TryAdd(audioClipType, new Queue<PoolableAudioSourceController>());
            _loopedAudioSources[audioClipType].Enqueue(audioSource);
            _loopedAudioCounter[audioClipType] += 1;
        }
        
        public void StopSoundInLoop(AudioClipType audioClipType)
        {
            if (!_loopedAudioCounter.TryGetValue(audioClipType, out var audioCounter))
            {
                return;
            }
            
            if (audioCounter <= 0)
            {
                return;
            }
            
            var audioSource = _loopedAudioSources[audioClipType].Dequeue();
            if (audioSource != null)
            {
                _audioSourcePool.Recycle(audioSource);
            }
            
            _loopedAudioCounter[audioClipType] -= 1;
            
            if (_loopedAudioCounter[audioClipType] == 0)
            {
                _loopedAudioCounter.Remove(audioClipType);
            }
        }
        
        private PoolableAudioSourceController GetAudioSource()
        {
            return _audioSourcePool.Spawn<PoolableAudioSourceController>((int)AudioSourcePoolId.PoolableAudioSource, true);
        }
        
        private AudioClip GetAudioClip(AudioClipType type)
        {
            return _assetManager.GetAudioAssets().GetAudioClip(type);
        }
    }
}
