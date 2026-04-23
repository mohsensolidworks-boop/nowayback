using System.Collections.Generic;
using UnityEngine;

namespace Main.Infrastructure.Context
{
    public abstract class AContextFlow
    {
        private readonly Dictionary<string, IContextUnit> _units;
        private readonly List<IContextUnit> _sortedUnits;
        private readonly List<ICoreContextUnit> _coreContextUnits;
        private readonly List<IContextBehaviour> _behaviours;
        private bool _isPaused;
        
        protected AContextFlow()
        {
            _units = new Dictionary<string, IContextUnit>();
            _sortedUnits = new List<IContextUnit>();
            _coreContextUnits = new List<ICoreContextUnit>();
            _behaviours = new List<IContextBehaviour>();
            
            var contextUnits = GetContextUnits();
            for (var i = 0; i < contextUnits.Length; i++)
            {
                Add(contextUnits[i]);
            }
        }
        
        protected abstract IContextUnit[] GetContextUnits();
        protected abstract void StartFlow();
        protected abstract void PauseFlow();
        protected abstract void ResumeFlow();
        protected abstract void EndFlow();
        
        #region Controller
        
        public void Activate()
        {
            BindUnits();
            StartFlow();
        }
        
        public void ApplyPause(bool isPaused)
        {
            if (isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
        
        private void Pause()
        {
            if (_isPaused)
            {
                return;
            }
            
            _isPaused = true;
            PauseFlow();
        }
        
        private void Resume()
        {
            if (!_isPaused)
            {
                return;
            }
            
            _isPaused = false;
            ResumeFlow();
        }
        
        public void Deactivate()
        {
            EndFlow();
        }
        
        #endregion
        
        #region Container
        
        private void BindUnits()
        {
            for (var i = 0; i < _sortedUnits.Count; i++)
            {
                _sortedUnits[i].Bind();
            }
        }
        
        public void OnActivateScene()
        {
            for (var i = 0; i < _coreContextUnits.Count; i++)
            {
                _coreContextUnits[i].OnActivateScene();
            }
        }
        
        public void ManualFixedUpdate()
        {
            for (var i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].ManualFixedUpdate();
            }
        }
        
        public void ManualUpdate()
        {
            for (var i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].ManualUpdate();
            }
        }
        
        public void ManualLateUpdate()
        {
            for (var i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].ManualLateUpdate();
            }
        }
        
        public void ManualGizmos()
        {
            for (var i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].ManualGizmos();
            }
        }
        
        private void Add(IContextUnit unit)
        {
            var id = unit.GetType().Name;
            _units[id] = unit;
            _sortedUnits.Add(unit);
            
            if (unit is ICoreContextUnit interUnit)
            {
                _coreContextUnits.Add(interUnit);
            }
            
            if (unit is IContextBehaviour behaviour)
            {
                _behaviours.Add(behaviour);
            }
        }
        
        public T Get<T>() where T : IContextUnit
        {
            var id = typeof(T).Name;
            var unit = (T)_units[id];
            if (unit == null)
            {
                Debug.LogError($"Missing ContextUnitId: {id}");
            }
            
            return unit;
        }
        
        #endregion
    }
}
