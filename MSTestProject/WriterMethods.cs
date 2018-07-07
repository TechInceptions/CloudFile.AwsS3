using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECHIS.CloudFile.S3;

namespace MSTestProject
{
    public class WriterMethods
    {
        public async static Task<List<string>> WriteFilesAsync(string rootPath, string stringData, int fileCount)
        {
            List<string> paths = new List<string>();
            for (int i = 0; i < fileCount; i++)
            {
                string path = $"{rootPath}/f{i}";
                await WriteTextAsync(path, stringData);
                paths.Add(path);
            }

            return paths;
        }
        public static List<string> WriteFiles(string rootPath, string stringData, int fileCount)
        {
            List<string> paths = new List<string>();
            for (int i = 0; i < fileCount; i++)
            {
                string path = $"{rootPath}/f{i}";
                WriteText(path, stringData);
                paths.Add(path);
            }

            return paths;
        }
        public static void WriteText(string filePath, string stringData)
        {

            var containerUri = GetConnectionString();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(stringData);
            (new Writer()).Connect(containerUri).WriteToBlob(data, filePath);

        }
        public static void WriteTextWithEncoding(string filePath, string stringData, Encoding e)
        {

            var containerUri = GetConnectionString();
            byte[] data = e.GetBytes(stringData);
            (new Writer()).Connect(containerUri, e).WriteToBlob(data, filePath);
        }
        public static async Task WriteTextWithEncodingAsync(string filePath, string stringData, Encoding e)
        {
            var containerUri = GetConnectionString();
            byte[] data = e.GetBytes(stringData);
            await (new Writer()).Connect(containerUri, e).WriteToBlobAsync(data, filePath);
        }
        public static async Task WriteTextAsync(string filePath, string stringData)
        {

            var containerUri = GetConnectionString();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(stringData);
            await (new Writer()).Connect(containerUri).WriteToBlobAsync(data, filePath);

        }
        public static string GetConnectionString()
        {
            //You need to set this!
            //BucketName|RegionName|SecretAccessKey|AccessKey|ProfileName
            return null;
        }
    }
}
