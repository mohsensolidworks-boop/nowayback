using Main.Infrastructure.Context;
using Main.Infrastructure.Utils;

namespace Main.Context.Scenes.Gameplay.General
{
    public sealed class GameplayOperationsManager : ISceneContextUnit
    {
        public bool HasOperation => !_operationCounter.IsEnabled();
        
        private EnableCounter _operationCounter;
        
        public void Bind()
        {
            _operationCounter = new EnableCounter();
        }
        
        public void AddOperation()
        {
            _operationCounter.Disable();
        }
        
        public void RemoveOperation()
        {
            _operationCounter.Enable();
        }
    }
}
