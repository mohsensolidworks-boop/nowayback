using System;
using System.Collections.Generic;

namespace Main.Context.Core.General
{
    public static class AppDelegates
    {
        public delegate void AppLifecycleDelegate();
        
        public static event AppLifecycleDelegate OnApplicationStart
        {
            add
            {
                if (!_START_LISTENERS.Contains(value))
                {
                    _START_LISTENERS.Add(value);
                }
            }

            remove
            {
                _START_LISTENERS.Remove(value);
            }
        }
        
        public static event AppLifecycleDelegate OnApplicationPause
        {
            add
            {
                if (!_PAUSE_LISTENERS.Contains(value))
                {
                    _PAUSE_LISTENERS.Add(value);
                }
            }
            remove
            {
                _PAUSE_LISTENERS.Remove(value);
            }
        }
        
        public static event AppLifecycleDelegate OnApplicationResume
        {
            add
            {
                if (!_RESUME_LISTENERS.Contains(value))
                {
                    _RESUME_LISTENERS.Add(value);
                }
            }
            remove
            {
                _RESUME_LISTENERS.Remove(value);
            }
        }
        
        public static event AppLifecycleDelegate OnApplicationQuit
        {
            add
            {
                if (!_QUIT_LISTENERS.Contains(value))
                {
                    _QUIT_LISTENERS.Add(value);
                }
            }

            remove
            {
                _QUIT_LISTENERS.Remove(value);
            }
        }
        
        private static readonly List<AppLifecycleDelegate> _START_LISTENERS;
        private static readonly List<AppLifecycleDelegate> _PAUSE_LISTENERS;
        private static readonly List<AppLifecycleDelegate> _RESUME_LISTENERS;
        private static readonly List<AppLifecycleDelegate> _QUIT_LISTENERS;
        private static readonly List<AppLifecycleDelegate> _MISSING_METHODS;
        private static readonly List<AppLifecycleDelegate> _TEMPORARY_LIST_HOLDER;
        
        static AppDelegates()
        {
            _START_LISTENERS = new List<AppLifecycleDelegate>();
            _PAUSE_LISTENERS = new List<AppLifecycleDelegate>();
            _RESUME_LISTENERS = new List<AppLifecycleDelegate>();
            _QUIT_LISTENERS = new List<AppLifecycleDelegate>();
            _MISSING_METHODS = new List<AppLifecycleDelegate>();
            _TEMPORARY_LIST_HOLDER = new List<AppLifecycleDelegate>();
        }
        
        public static void CallStartListeners()
        {
            InvokeListenerMethod(_START_LISTENERS);
        }
        
        public static void CallPauseListeners()
        {
            InvokeListenerMethod(_PAUSE_LISTENERS);
        }

        public static void CallResumeListeners()
        {
            InvokeListenerMethod(_RESUME_LISTENERS);
        }
        
        public static void CallQuitListeners()
        {
            InvokeListenerMethod(_QUIT_LISTENERS);
        }
        
        public static void Clear()
        {
            _START_LISTENERS.Clear();
            _PAUSE_LISTENERS.Clear();
            _RESUME_LISTENERS.Clear();
            _QUIT_LISTENERS.Clear();
        }
        
        private static void InvokeListenerMethod(List<AppLifecycleDelegate> listeners)
        {
            _TEMPORARY_LIST_HOLDER.Clear();
            _TEMPORARY_LIST_HOLDER.AddRange(listeners);
            _MISSING_METHODS.Clear();
            
            for (var i = 0; i < _TEMPORARY_LIST_HOLDER.Count; i++)
            {
                try
                {
                    var method = _TEMPORARY_LIST_HOLDER[i];
                    method.Invoke();
                }
                catch (Exception)
                {
                    var method = _TEMPORARY_LIST_HOLDER[i];
                    _MISSING_METHODS.Add(method);
                }
            }
            
            if (_MISSING_METHODS.Count > 0)
            {
                listeners.RemoveAll(_MISSING_METHODS.Contains);
                _MISSING_METHODS.Clear();
            }
        }
    }
}
