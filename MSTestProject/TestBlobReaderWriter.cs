using Microsoft.VisualStudio.TestTools.UnitTesting;
using TECHIS.CloudFile;
using System.Linq;
using TECHIS.CloudFile.S3;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System;


namespace MSTestProject
{
    [TestClass]
    public class TestBlobReaderWriter
    {
        private string FixedFileName => "FixedDirectory/S2715H.inf";

        [TestMethod]
        public void WriteReadText()
        {
            string input = $"Written by {nameof(WriteReadText)}";
            WriterMethods.WriteText(FixedFileName, input);

            string data = (new Reader()).Connect(WriterMethods.GetConnectionString()).ReadText(FixedFileName);

            Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
            Assert.IsTrue(string.Equals( input, data), "input doesn't match output");
        }

        [TestMethod]
        public async void WriteReadTextAsync()
        {
            string input = $"Written by {nameof(WriteReadText)}";
            await WriterMethods.WriteTextAsync(FixedFileName, input);

            string data = await (new Reader()).Connect(WriterMethods.GetConnectionString()).ReadTextAsync(FixedFileName);

            Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
            Assert.IsTrue(string.Equals(input, data), "input doesn't match output");
        }



        [TestMethod]
        public void WriteReadData2()
        {
            Encoding e = Encoding.UTF8;
            string input = $"Written by {nameof(WriteReadText)}";
            WriterMethods.WriteTextWithEncoding(FixedFileName, input, e);

            string data;
            using (MemoryStream ms = new MemoryStream())
            {
                (new Reader()).Connect(WriterMethods.GetConnectionString(), e).ReadData(FixedFileName,ms);
                 data = new string(e.GetChars(ms.ToArray()));
            }

            Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
            Assert.IsTrue(string.Equals(input, data), "input doesn't match output");
            
        }

        [TestMethod]
        public async void WriteReadData2Async()
        {
            Encoding e = Encoding.UTF8;
            string input = $"Written by {nameof(WriteReadText)}";
            await WriterMethods.WriteTextWithEncodingAsync(FixedFileName, input, e);

            string data;
            using (MemoryStream ms = new MemoryStream())
            {
                await (new Reader()).Connect(WriterMethods.GetConnectionString(), e).ReadDataAsync(FixedFileName, ms);
                data = new string(e.GetChars(ms.ToArray()));
            }

            Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
            Assert.IsTrue(string.Equals(input, data), "input doesn't match output");

        }
       



    }
}
