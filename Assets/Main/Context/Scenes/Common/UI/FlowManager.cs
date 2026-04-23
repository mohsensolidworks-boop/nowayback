using System.Collections.Generic;
using Main.Infrastructure.Context;

namespace Main.Context.Scenes.Common.UI
{
    public sealed class FlowManager : ISceneContextUnit, IContextBehaviour
    {
        private LinkedList<FlowAction> _actions;
        private FlowAction _runningAction;
        
        public void Bind()
        {
            _actions = new LinkedList<FlowAction>();
        }
        
        public void ManualUpdate()
        {
            TryExecuteNextAction();
        }
        
        private void TryExecuteNextAction()
        {
            if (_runningAction != null)
            {
                return;
            }
            
            if (_actions.Count > 0)
            {
                _runningAction = _actions.First.Value;
                _actions.RemoveFirst();
                _runningAction.OnKilled += OnActionKilled;
                _runningAction.Execute();
            }
        }
        
        private void OnActionKilled()
        {
            _runningAction.OnKilled -= OnActionKilled;
            _runningAction = null;
            
            TryExecuteNextAction();
        }
        
        public void Push(FlowAction action)
        {
            if (_runningAction == null || _runningAction.GetType() != action.GetType())
            {
                if (!_actions.Contains(action))
                {
                    _actions.AddLast(action);
                }
            }
            
            TryExecuteNextAction();
        }
    }
}
