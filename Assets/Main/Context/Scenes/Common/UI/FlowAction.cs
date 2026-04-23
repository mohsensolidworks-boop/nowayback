using System;

namespace Main.Context.Scenes.Common.UI
{
    public abstract class FlowAction
    {
        public event Action OnCompleted;
        public event Action OnKilled;
        
        public abstract void Execute();
        
        public void Cancel()
        {
            OnCancel();
            Kill();
        }
        
        public void Complete()
        {
            OnComplete();
            OnCompleted?.Invoke();
            Kill();
        }
        
        private void Kill()
        {
            OnKill();
            OnKilled?.Invoke();
        }
        
        protected abstract void OnCancel();
        protected abstract void OnComplete();
        protected abstract void OnKill();
    }
}
