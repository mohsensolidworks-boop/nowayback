using UnityEngine;

namespace Main.Infrastructure.General
{
    public abstract class ManualBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
        }
        
        protected virtual void Update()
        {
        }
        
        protected virtual void LateUpdate()
        {
        }
        
        protected virtual void FixedUpdate()
        {
        }
        
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            
            ManualGizmos();
        }
        
        protected virtual void ManualGizmos()
        {
        }
        
        public static void Destroy(GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }
    }
}
