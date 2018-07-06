using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace TECHIS.CloudFile.S3
{
    public class Writer : ICloudFileWriterFactory, ICloudFileWriter
    {
        private ConnectionInfo _ConnectionInfo;
        private AmazonS3Client _S3Client;

        public ICloudFileWriter Connect(string pipeSeperatedConnectionInfo, Encoding encoding = null)
        {
            _S3Client = (_ConnectionInfo = ConnectionInfo.Get(pipeSeperatedConnectionInfo)).Connect();
            return this;
        }

        public ICloudFileWriter Connect(string pipeSeperatedConnectionInfo, string connectionDetails, Encoding encoding = null)
        {
            _S3Client = (_ConnectionInfo = ConnectionInfo.Get(pipeSeperatedConnectionInfo)).Connect();
            return this;
        }

        public void WriteToBlob(Stream ms, string fileName)
        {
            throw new NotImplementedException();
        }

        public void WriteToBlob(byte[] data, string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task WriteToBlobAsync(Stream ms, string fileName)
        {
            // simple object put
            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = _ConnectionInfo.BucketName,
                Key = fileName,
                InputStream = ms
            };

            PutObjectResponse response = await _S3Client.PutObjectAsync(request).ConfigureAwait(false);
            

            await _S3Client.PutObjectAsync(request);
        }

        public async Task WriteToBlobAsync(byte[] data, string fileName)
        {
            await WriteToBlobAsync(new MemoryStream(data), fileName);
        }
    }
}
