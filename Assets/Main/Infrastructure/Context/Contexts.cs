using System;
using System.Collections;
using Main.Infrastructure.General;
using UnityEngine;

namespace Main.Infrastructure.Context
{
    public static class Contexts
    {
        private static readonly Type _CORE_CONTEXT_UNIT;
        private static readonly Type _SCENE_CONTEXT_UNIT;
        private static AContextFlow _CORE;
        private static AContextFlow _SCENE;
        private static bool _HAS_CORE;
        private static bool _HAS_SCENE;
        private static ManualBehaviour _MANUAL_BEHAVIOUR; 
        
        static Contexts()
        {
            _CORE_CONTEXT_UNIT = typeof(ICoreContextUnit);
            _SCENE_CONTEXT_UNIT = typeof(ISceneContextUnit);
        }
        
        public static T Get<T>() where T : IContextUnit
        {
            var type = typeof(T);
            if (_CORE_CONTEXT_UNIT.IsAssignableFrom(type))
            {
                return _CORE.Get<T>();
            }
            else if (_SCENE_CONTEXT_UNIT.IsAssignableFrom(type))
            {
                return _SCENE.Get<T>();
            }
            
            return default;
        }
        
        public static Coroutine ManualStartCoroutine(IEnumerator iEnumerator)
        {
            return _MANUAL_BEHAVIOUR.StartCoroutine(iEnumerator);
        }
        
        public static void ManualStopCoroutine(Coroutine coroutine)
        {
            _MANUAL_BEHAVIOUR.StopCoroutine(coroutine);
        }
        
        public static void ActivateCoreContext(AContextFlow contextFlow, ManualBehaviour manualBehaviour)
        {
            _MANUAL_BEHAVIOUR = manualBehaviour;
            _HAS_CORE = true;
            _CORE = contextFlow;
            _CORE.Activate();
        }
        
        public static void DeactivateCoreContext()
        {
            if (!_HAS_CORE)
            {
                return;
            }
            
            _HAS_CORE = false;
            _CORE.Deactivate();
            _CORE = null;
        }
        
        public static void ApplyPause(bool isPaused)
        {
            if (_HAS_CORE)
            {
                _CORE.ApplyPause(isPaused);
            }
        }
        
        public static void ActivateSceneContext(AContextFlow contextFlow)
        {
            _CORE.OnActivateScene();
            
            _HAS_SCENE = true;
            _SCENE = contextFlow;
            _SCENE.Activate();
        }
        
        public static void DeactivateSceneContext()
        {
            if (!_HAS_SCENE)
            {
                return;
            }
            
            _HAS_SCENE = false;
            _SCENE.Deactivate();
            _SCENE = null;
        }
        
        public static void MasterFixedUpdate()
        {
            _CORE.ManualFixedUpdate();
            
            if (_HAS_SCENE)
            {
                _SCENE.ManualFixedUpdate();
            }
        }
        
        public static void MasterUpdate()
        {
            _CORE.ManualUpdate();
            
            if (_HAS_SCENE)
            {
                _SCENE.ManualUpdate();
            }
        }
        
        public static void MasterLateUpdate()
        {
            _CORE.ManualLateUpdate();
            
            if (_HAS_SCENE)
            {
                _SCENE.ManualLateUpdate();
            }
        }
        
        public static void MasterGizmos()
        {
            _CORE.ManualGizmos();
            
            if (_HAS_SCENE)
            {
                _SCENE.ManualGizmos();
            }
        }
    }
}
