using Main.Infrastructure.General;

namespace Main.Context.Core.Audio
{
    public class PoolableAudioSourceController : AudioSourceController, IPoolable
    {
        public int GetPoolId()
        {
            return (int)AudioSourcePoolId.PoolableAudioSource;
        }
        
        public void OnSpawn()
        {
        }
        
        public void OnRecycle()
        {
            Reset();
        }
    }
    
    public enum AudioSourcePoolId
    {
        PoolableAudioSource
    }
}
