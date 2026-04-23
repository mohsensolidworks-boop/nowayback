using Main.Infrastructure.General;
using Main.Context.Core.General;

namespace Main.Infrastructure.Context
{
    public class ContextController : ManualBehaviour
    {
        protected override void Awake()
        {
            DontDestroyOnLoad(this);
            
            Contexts.ActivateCoreContext(new CoreContextFlow(), this);
        }
        
        protected override void FixedUpdate()
        {
            Contexts.MasterFixedUpdate();
        }
        
        protected override void Update()
        {
            Contexts.MasterUpdate();
        }
        
        protected override void LateUpdate()
        {
            Contexts.MasterLateUpdate();
        }
        
        protected override void ManualGizmos()
        {
            Contexts.MasterGizmos();
        }
        
        private void OnApplicationPause(bool isPaused)
        {
            Contexts.ApplyPause(isPaused);
        }
        
        private void OnApplicationQuit()
        {
            Contexts.DeactivateCoreContext();
        }
        
        private void OnDestroy()
        {
            Contexts.DeactivateCoreContext();
        }
    }
}
