using Main.Infrastructure.General;
using UnityEngine;

namespace Main.Context.Core.Audio
{
    public class AudioSourceController : ManualBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        
        public void Mute()
        {
            _audioSource.mute = true;
        }
        
        public void Unmute()
        {
            _audioSource.mute = false;
        }
        
        public bool IsPlaying()
        {
            return _audioSource.isPlaying;
        }
        
        public void PlayOneShot(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }

        public void PlayLoop(AudioClip clip)
        {
            _audioSource.loop = true;
            _audioSource.clip = clip;
            _audioSource.Play();
        }
        
        public void Stop()
        {
            _audioSource.Stop();
        }
        
        protected void Reset()
        {
            _audioSource.Stop();
            _audioSource.volume = 1f;
            _audioSource.clip = null;
            Unmute();
        }
    }
}
