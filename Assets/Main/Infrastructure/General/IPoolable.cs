namespace Main.Infrastructure.General
{
    public interface IPoolable
    {
        public int GetPoolId();
        public void OnSpawn();
        public void OnRecycle();
    }
}
