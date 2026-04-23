using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using Main.Infrastructure.Utils;
using UnityEngine;

namespace Main.Context.Core.Logger
{
    public class LogWriter
    {
        private const string _PROJECT_NAME = "junction";
        private const int _MAX_FILE_SIZE = 1024 * 1024 * 2; // 2 MB
        private const string _WRITER_THREAD_NAME = "LogWriterThread";
        
        private static string _DIRECTORY_PATH;
        
        private readonly string _path1;
        private readonly string _path2;
        private readonly string _zipPath;
        private readonly StringBuilder _logBuilder;
        private readonly StringBuilder _crashlyticsLogBuilder;
        private readonly ConcurrentQueue<LogLine> _logLineQueue;
        private Thread _writerThread;
        private bool _isRunning;
        private bool _shouldCompress;
        private bool _shouldPause;
        
        public LogWriter()
        {
            _DIRECTORY_PATH = Path.Combine(FileHelper.PERSISTENT_DATA_PATH, "Logs");
            _path1 = Path.Combine(_DIRECTORY_PATH, $"{_PROJECT_NAME}.log");
            _path2 = Path.Combine(_DIRECTORY_PATH, $"{_PROJECT_NAME}_old.log");
            _zipPath = Path.Combine(FileHelper.PERSISTENT_DATA_PATH, "Logs.zip");
            _logBuilder = new StringBuilder();
            _crashlyticsLogBuilder = new StringBuilder();
            _logLineQueue = new ConcurrentQueue<LogLine>();
            
            if (!Directory.Exists(_DIRECTORY_PATH))
            {
                Directory.CreateDirectory(_DIRECTORY_PATH);
            }
            
            Start();
        }

        public void Start()
        {
            _shouldPause = false;

            if (_isRunning)
            {
                return;
            }

            _isRunning = true;
            _writerThread = new Thread(WriterLoop)
            {
                Name = _WRITER_THREAD_NAME, IsBackground = true
            };

            _writerThread.Start();
        }

        public void Stop()
        {
            _shouldPause = true;
            Pause();
            Write();
        }

        public void CallPause()
        {
            _shouldPause = true;
        }

        private void Pause()
        {
            _isRunning = false;
            _writerThread = null;
        }

        public void CallSaveAndCompress()
        {
            _shouldCompress = true;
        }

        public void AddLine(LogLine line)
        {
            _logLineQueue.Enqueue(line);
        }

        private void WriterLoop()
        {
            while (_isRunning)
            {
                Write();
                
                if (_shouldCompress)
                {
                    FileHelper.Compress(_DIRECTORY_PATH, _zipPath);
                    _shouldCompress = false;
                }

                if (_shouldPause)
                {
                    Pause();
                }

                Thread.Sleep(1000);
            }
        }

        private void Write()
        {
            BuildString();
            WriteToFile();
        }

        private void BuildString()
        {
            while (_logLineQueue.TryDequeue(out var logLine))
            {
                logLine.LineToString(_logBuilder);
                SendCrashlyticsLog(logLine);
            }
        }

        private void SendCrashlyticsLog(LogLine logLine)
        {
            lock (_crashlyticsLogBuilder)
            {
                logLine.LineToString(_crashlyticsLogBuilder);
                _crashlyticsLogBuilder.Clear();
            }
        }

        private void WriteToFile()
        {
            var shouldMoveFile = false;

            try
            {
                using var streamWriter = File.AppendText(_path1);
                streamWriter.Write(_logBuilder.ToString());
                shouldMoveFile = streamWriter.BaseStream.Length >= _MAX_FILE_SIZE;
                _logBuilder.Clear();
            }
            catch (Exception ex)
            {
                Debug.LogError("LogWriter.WriteToFile():" + ex);
            }

            if (shouldMoveFile)
            {
                MoveFile();
            }
        }

        private void MoveFile()
        {
            try
            {
                if (File.Exists(_path2))
                {
                    File.Delete(_path2);
                }

                File.Move(_path1, _path2);
            }
            catch (Exception ex)
            {
                Debug.LogError("LogWriter.MoveFile():" + ex);
            }
        }
    }
}
