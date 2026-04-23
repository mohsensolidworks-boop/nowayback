using Main.Context.Scenes.Common.General;
using Main.Infrastructure.Context;

namespace Main.Context.Scenes.Home.General
{
    public sealed class HomeInputIterator : ISceneContextUnit, IContextBehaviour
    {
        private UIInputListener _uiInputListener;
        private InputListener _inputListener;
        
        public void Bind()
        {
            _uiInputListener = Contexts.Get<UIInputListener>();
            _inputListener = Contexts.Get<InputListener>();
        }
        
        public void ManualUpdate()
        {
            _uiInputListener.ControlInputs();
            _inputListener.ControlInputs();
        }
    }
}
