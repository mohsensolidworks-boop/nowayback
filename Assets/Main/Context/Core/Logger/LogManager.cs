using System;
using System.Text;
using System.Threading;
using Main.Context.Core.General;
using Main.Infrastructure.Context;
using UnityEngine;

namespace Main.Context.Core.Logger
{
    public sealed class LogManager : ICoreContextUnit
    {
        private static StringBuilder _UNITY_LOG_BUILDER;
        
        private readonly LogWriter _logWriter;
        private readonly Thread _mainThread;
        
        public LogManager()
        {
            Application.logMessageReceived += HandleLog;
            
            _UNITY_LOG_BUILDER = new StringBuilder();
            _logWriter = new LogWriter();
            _mainThread = Thread.CurrentThread;
            
            Log.Init(this);
        }
        
        public void Bind()
        {
            AppDelegates.OnApplicationPause += OnPause;
            AppDelegates.OnApplicationResume += OnFocus;
            AppDelegates.OnApplicationQuit += OnCoreContextDeactivate;
        }
        
        public void OnActivateScene()
        {
        }
        
        private void OnCoreContextDeactivate()
        {
            Stop();
        }
        
        private static void HandleLog(string logString, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Warning:
                    Log.Warning(typeof(LogManager), LogTag.Unity, logString);
                    break;
                case LogType.Error or LogType.Exception:
                    Log.Error(typeof(LogManager), LogTag.Unity, PrepareUnityLogStringWithStackTrace(logString, stackTrace));
                    break;
            }
        }
        
        private static string PrepareUnityLogStringWithStackTrace(string logString, string stackTrace)
        {
            if (stackTrace.Length <= 0)
            {
                return logString;
            }
            
            _UNITY_LOG_BUILDER.Clear();
            _UNITY_LOG_BUILDER.AppendLine(logString);
            _UNITY_LOG_BUILDER.Append(stackTrace);
            return _UNITY_LOG_BUILDER.ToString();
        } 

        private void OnPause()
        {
            _logWriter.CallPause();
        }

        private void OnFocus()
        {
            _logWriter.Start();
        }
        
        public void SaveAndCompress()
        {
            _logWriter.CallSaveAndCompress();
        }

        public void AddLine(Type classType, LogLevel level, LogTag tag, string message)
        {
            var frameCount = -1;

            if (_mainThread.Equals(Thread.CurrentThread))
            {
                frameCount = Time.frameCount;
            }

            var line = new LogLine
            {
                DateTime = DateTime.Now.ToUniversalTime(),
                FrameCount = frameCount,
                LOGLevel = level,
                LOGTag = tag,
                ClassType = classType,
                Message = message,
            };

            _logWriter.AddLine(line);
        }

        private void Stop()
        {
            AppDelegates.OnApplicationPause -= OnPause;
            AppDelegates.OnApplicationResume -= OnFocus;
            Application.logMessageReceived -= HandleLog;
            
            _logWriter.Stop();
        }
    }
}
