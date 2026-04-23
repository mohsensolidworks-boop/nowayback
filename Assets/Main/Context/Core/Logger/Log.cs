using System;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Main.Context.Core.Logger
{
    public static class Log
    {
        private static LogManager _LOG_MANAGER;
        
        public static void Init(LogManager logManager)
        {
            _LOG_MANAGER = logManager;
        }
        
        public static void DebugTMP(string log)
        {
            Debug(typeof(Log), LogTag.Default, log);
        }
        
        public static void Debug([NotNull] object callerClass, LogTag logTag, [NotNull] string log)
        {
#if UNITY_EDITOR
            if (_LOG_MANAGER == null)
            {
                var callerClassName = callerClass.ToString().Split('.')[^1];
                var message = ToMessage(logTag, callerClassName, log);
                UnityEngine.Debug.Log(message);
                return;
            }
            else if (logTag != LogTag.Unity)
            {
                var callerClassName = callerClass.GetType().Name;
                var message = ToMessage(logTag, callerClassName, log);
                UnityEngine.Debug.Log(message);
            }
#endif
            
            _LOG_MANAGER.AddLine(callerClass.GetType(), LogLevel.Debug, logTag, log);
        }
        
        public static void Warning(object callerClass, LogTag logTag, string log)
        {
#if UNITY_EDITOR
            if (_LOG_MANAGER == null)
            {
                var callerClassName = callerClass.ToString().Split('.')[^1];
                var message = ToMessage(logTag, callerClassName, log);
                UnityEngine.Debug.LogWarning(message);
                return;
            }
            else if (logTag != LogTag.Unity)
            {
                var callerClassName = callerClass.GetType().Name;
                var message = ToMessage(logTag, callerClassName, log);
                UnityEngine.Debug.LogWarning(message);
            }
#endif
            
            _LOG_MANAGER.AddLine(callerClass.GetType(), LogLevel.Warning, logTag, log);
        }
        
        public static void Error(object callerClass, LogTag logTag, string log)
        {
#if UNITY_EDITOR
            if (_LOG_MANAGER == null)
            {
                var callerClassName = callerClass.ToString().Split('.')[^1];
                var message = ToMessage(logTag, callerClassName, log);
                UnityEngine.Debug.LogError(message);
                return;
            }
            else if (logTag != LogTag.Unity)
            {
                var callerClassName = callerClass.GetType().Name;
                var message = ToMessage(logTag, callerClassName, log);
                UnityEngine.Debug.LogError(message);
            }
#endif
            
            _LOG_MANAGER.AddLine(callerClass.GetType(), LogLevel.Error, logTag, log);
        }
        
        private static object ToMessage(params object[] parameters)
        {
            var log = new StringBuilder();
            log.Append(Time.frameCount);
            
            for (var i = 0; i < parameters.Length; i++)
            {
                log.Append(" | ").Append(parameters[i]);
            }
            
            return log;
        }
    }
    
    public struct LogLine
    {
        public DateTime DateTime;
        public int FrameCount;
        public LogLevel LOGLevel;
        public LogTag LOGTag;
        public Type ClassType;
        public string Message;
        
        private const string _DELIMITER = " | ";
        private const string _PRE_MESSAGE_DELIMITER = " > ";
        private const string _TIME_FORMAT = "yy/MM/dd HH:mm:ss.fff";

        public void LineToString(StringBuilder builder)
        {
            builder.Append(DateTime.ToString(_TIME_FORMAT, CultureInfo.InvariantCulture)).Append(_DELIMITER)
                   .Append(FrameCount).Append(_DELIMITER)
                   .Append(LOGLevel.ToString()[0]).Append(_DELIMITER)
                   .Append(LOGTag.ToString()).Append(_DELIMITER)
                   .Append(ClassType.Name).Append(_PRE_MESSAGE_DELIMITER);
            
            builder.AppendLine(Message);    
        }
    }
    
    public enum LogLevel
    {
        Debug,
        Error,
        Warning
    }
    
    public enum LogTag
    {
        Default,
        App,
        UI,
        Native,
        Unity,
        User,
        Gameplay,
        GameState,
        Haptic,
        Audio,
        Download,
        Bundle,
        Dialog,
    }
}
