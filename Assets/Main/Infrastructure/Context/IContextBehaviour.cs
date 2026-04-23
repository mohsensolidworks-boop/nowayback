namespace Main.Infrastructure.Context
{
    public interface IContextBehaviour
    {
        public void ManualUpdate();
        
        public void ManualFixedUpdate()
        {
        }
        
        public void ManualLateUpdate()
        {
        }
        
        public void ManualGizmos()
        {
        }
    }
}
