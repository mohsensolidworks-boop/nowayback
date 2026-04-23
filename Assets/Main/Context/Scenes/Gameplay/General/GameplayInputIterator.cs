using Main.Context.Scenes.Common.General;
using Main.Infrastructure.Context;

namespace Main.Context.Scenes.Gameplay.General
{
    public sealed class GameplayInputIterator : ISceneContextUnit, IContextBehaviour
    {
        private UIInputListener _uiInputListener;
        private InputListener _inputListener;
        private GameplayOtherInputListener _gameplayOtherInputListener;
        
        public void Bind()
        {
            _uiInputListener = Contexts.Get<UIInputListener>();
            _inputListener = Contexts.Get<InputListener>();
            _gameplayOtherInputListener = Contexts.Get<GameplayOtherInputListener>();
        }
        
        public void ManualUpdate()
        {
            _uiInputListener.ControlInputs();
            _inputListener.ControlInputs();
            _gameplayOtherInputListener.ControlInputs();
        }
    }
}
