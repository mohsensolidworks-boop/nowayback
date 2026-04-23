using Main.Infrastructure.Context;
using System.Collections;
using System.IO;
using System.Text;
using Main.Context.Core.Logger;
using UnityEngine.Networking;
using Application = UnityEngine.Application;

namespace Main.Context.Core.General
{
    public sealed class FileDownloadManager : ICoreContextUnit
    {
        private const string _BASE_URL = "https://8ec21ded-5f41-4468-b1a9-59ed0b8c287c.client-api.unity3dusercontent.com/client_api/v1/environments/production/buckets/74af3a00-18e9-4e18-a542-7f42177e607e/release_by_badge/latest/entry_by_path/content/?path=";
        
        private string _fileSaveDirectory;
        
        public void Bind()
        {
            _fileSaveDirectory = Application.persistentDataPath;
            
            if (!Directory.Exists(_fileSaveDirectory))
            {
                Directory.CreateDirectory(_fileSaveDirectory);
            }
        }
        
        public void OnActivateScene()
        {
        }
        
        private IEnumerator DownloadFile(string fileName)
        {
            var url = _BASE_URL + fileName;
            var request = UnityWebRequest.Get(url);
            try
            {
                // ETag gönder (If-None-Match)
                var etagFile = ETagPath(fileName);
                if (File.Exists(etagFile))
                {
                    var etag = File.ReadAllText(etagFile);
                    if (!string.IsNullOrEmpty(etag))
                    {
                        request.SetRequestHeader("If-None-Match", etag);
                    }
                }

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    // 200 OK → yeni içerik
                    var json = request.downloadHandler.text;
                    File.WriteAllText(GetFileSavePath(fileName), json, Encoding.UTF8);

                    var etag = request.GetResponseHeader("ETag");
                    if (!string.IsNullOrEmpty(etag))
                    {
                        File.WriteAllText(etagFile, etag);
                    }

                    Log.Debug(this, LogTag.Download, $"Updated: {fileName}");
                    yield break;
                }

                if ((int)request.responseCode == 304)
                {
                    // 304 Not Modified → cache'i kullan
                    Log.Debug(this, LogTag.Download, $"Not modified: {fileName}");
                    yield break;
                }
                
                // Hata → varsa cache'e düş
                var cached = GetFileSavePath(fileName);
                if (File.Exists(cached))
                {
                    Log.Debug(this, LogTag.Download, $"Network error ({request.responseCode}): using cached {fileName}");
                }
                else
                {
                    Log.Debug(this, LogTag.Download, $"Failed and no cache for {fileName}: {request.error}");
                }
            }
            finally
            {
                request?.Dispose();
            }
        }

        private string ETagPath(string fileName)
        {
            return GetFileSavePath(fileName + ".etag");
        }

        private string GetFileSavePath(string fileName)
        {
            return Path.Combine(_fileSaveDirectory, fileName);
        }
    }
}
