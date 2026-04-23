using System.IO;
using System.IO.Compression;
using System.Text;

namespace Main.Infrastructure.Utils
{
    public static class FileHelper
    {
        public static readonly string PERSISTENT_DATA_PATH;
        
        static FileHelper()
        {
            PERSISTENT_DATA_PATH = Path.GetFullPath(UnityEngine.Application.persistentDataPath);
        }
        
        private static void CreateZipFile(string outputPath, string inputPath)
        {
            ZipFile.CreateFromDirectory(inputPath, outputPath, CompressionLevel.Optimal, false);
        }

        public static void Compress(string inputPath, string outputPath)
        {
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            CreateZipFile(outputPath, inputPath);
        }

        public static byte[] ZipStr(string str, Encoding encoding)
        {
            using var ms = new MemoryStream();
            using (var gs = new GZipStream(ms, CompressionMode.Compress))
            {
                using (var writer = new BinaryWriter(gs, encoding))
                {
                    writer.Write(str);
                }
            }

            ms.Flush();
            return ms.ToArray();
        }

        public static string UnzipStr(byte[] bytes, Encoding encoding)
        {
            using var ms = new MemoryStream(bytes);
            using var gs = new GZipStream(ms, CompressionMode.Decompress);
            using var reader = new BinaryReader(gs, encoding);
            var foo = reader.ReadString();
            return foo;
        }
    }
}
