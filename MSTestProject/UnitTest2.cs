using Microsoft.VisualStudio.TestTools.UnitTesting;
using TECHIS.CloudFile;
using System.Linq;
using TECHIS.CloudFile.S3;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System;

namespace Test.Cloud.AzureStorage
{
    [TestClass]
    public class TestBlobReaderWriter
    {
        [TestMethod]
        public void ReadText()
        {
            string data = (new Reader()).Connect(GetContainerUri()).ReadText(FixedFileName);

            Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
        }

        [TestMethod]
        public void WriteText()
        {

            var containerUri = GetContainerUri();
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes("Test data");
            (new Writer()).Connect(containerUri).WriteToBlob(data, fileName);

            //Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
        }
        [TestMethod]
        public void ReadTextAsync()
        {

            var containerUri = GetContainerUri();
            var fileName = FixedFileName;
            string data = (new Reader()).Connect(containerUri).ReadTextAsync(fileName).Result;

            Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
        }
        [TestMethod]
        public void ReadDataAsync()
        {
            Reader br = new Reader();
            var containerUri = GetContainerUri();
            var fileName = FixedFileName;
            string data = null;

            Encoding e = Encoding.UTF8;
            Task task = null;
            using (MemoryStream ms = new MemoryStream())
            {
                task = br.Connect(containerUri, e).ReadDataAsync(fileName, ms);
                task.Wait();
                data = new string(e.GetChars(ms.ToArray()));
            }
            Assert.IsFalse(string.IsNullOrEmpty(data), $"Failed to read blob file via {nameof(ReadDataAsync)}");
        }
        [TestMethod]
        public void ReadData()
        {
            Reader br = new Reader();
            var containerUri = GetContainerUri();
            var fileName = FixedFileName;
            string data = null;

            Encoding e = Encoding.UTF8;
            using (MemoryStream ms = new MemoryStream())
            {
                br.Connect(containerUri, e).ReadData(fileName, ms);
                data = new string(e.GetChars(ms.ToArray()));
            }
            Assert.IsFalse(string.IsNullOrEmpty(data), $"Failed to read blob file via {nameof(ReadData)}");
        }

        [TestMethod]
        public void WriteTextAsync()
        {
            Writer br = new Writer();
            string containerUri = GetContainerUri();
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes($"Test data: {DateTime.UtcNow.ToLongTimeString()}");
            var task = br.Connect(containerUri).WriteToBlobAsync(data, fileName);
            task.Wait();
            Assert.IsTrue(task.IsCompleted, "Failed to Write Async blob file");
        }
        [TestMethod]
        public void WriteTextAsync2()
        {
            Writer br = new Writer();
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes($"Test data: {DateTime.UtcNow.ToLongTimeString()}");
            var task = br.Connect(StorageConnectionString, "test").WriteToBlobAsync(data, fileName);
            task.Wait();
            Assert.IsTrue(task.IsCompleted, "Failed to Write Async blob file");
        }

        private string FixedFileName => "FixedDirectory/S2715H.inf";

        private static string GetContainerUri()
        {
            return "https://tests4dev.blob.core.windows.net/cloudfile?sv=2015-12-11&si=cloudfile-RWLD&sr=c&sig=qXnlg3DGNBrT8wWVAeeeqn8asP%2BJXjYdXH1os6mdaCU%3D";
        }

        private string StorageConnectionString => "SharedAccessSignature=sv=2016-05-31&ss=b&srt=co&sp=rwdl&st=2017-05-05T17%3A36%3A00Z&se=2018-05-06T17%3A36%3A00Z&sig=4qZAoOGRiuwK9THxy%2BTH4tLznQt%2FZIRESPvg%2Bc8qsTA%3D;BlobEndpoint=https://tests4dev.blob.core.windows.net/";

    }
}
