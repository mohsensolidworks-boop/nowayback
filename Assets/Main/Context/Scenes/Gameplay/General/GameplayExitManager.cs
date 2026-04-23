using Main.Context.Core.General;
using Main.Infrastructure.Context;

namespace Main.Context.Scenes.Gameplay.General
{
    public sealed class GameplayExitManager : ISceneContextUnit, IContextBehaviour
    {
        private GameplayOperationsManager _gameplayOperationsManager;
        
        private bool _isReturnHomeRequested;
        private bool _isReturnHomeTriggered;
        
        public void Bind()
        {
            _gameplayOperationsManager = Contexts.Get<GameplayOperationsManager>();
            
            _isReturnHomeRequested = false;
            _isReturnHomeTriggered = false;
        }
        
        public void RequestReturnHome(bool isWin)
        {
            if (_isReturnHomeRequested)
            {
                return;
            }
            
            _isReturnHomeRequested = true;
            
            if (isWin)
            {
                var userManager = Contexts.Get<UserManager>();
                userManager.IncrementLevel();
            }
        }
        
        public void ManualUpdate()
        {
            if (!_gameplayOperationsManager.HasOperation && _isReturnHomeRequested && !_isReturnHomeTriggered)
            {
                _isReturnHomeTriggered = true;
                Contexts.Get<SceneChangeManager>().LoadHomeScene();
            }
        }
    }
}
