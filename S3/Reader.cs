using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace TECHIS.CloudFile.S3
{
    public class Reader : ICloudFileReader, ICloudFileReaderFactory
    {
        private ConnectionInfo _ConnectionInfo;
        private AmazonS3Client _S3Client;

        public ICloudFileReader Connect(string pipeSeperatedConnectionInfo, Encoding encoding = null)
        {
            _S3Client = (_ConnectionInfo = ConnectionInfo.Get(pipeSeperatedConnectionInfo)).Connect();
            return this;
        }

        public ICloudFileReader Connect(string pipeSeperatedConnectionInfo, string connectionDetails, Encoding encoding = null)
        {
            _S3Client = (_ConnectionInfo = ConnectionInfo.Get(pipeSeperatedConnectionInfo)).Connect();
            return this;
        }

        public void ReadData(string fileName, Stream output)
        {
            ReadDataAsync(fileName, output).Wait();
        }

        public async Task ReadDataAsync(string fileName, Stream output)
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = _ConnectionInfo.BucketName,
                Key = fileName
            };


            GetObjectResponse response = await _S3Client.GetObjectAsync(request).ConfigureAwait(false);
            if (response!=null && response.ResponseStream!=null)
            {
                await response.ResponseStream.CopyToAsync(output);
            }
            
        }

        public string ReadText(string fileName)
        {
            return ReadTextAsync(fileName).Result;
        }

        public async Task<string> ReadTextAsync(string fileName)
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = _ConnectionInfo.BucketName,
                Key = fileName
            };

            string result = null;
            using (GetObjectResponse response = await _S3Client.GetObjectAsync(request).ConfigureAwait(false))
            {
                using (StreamReader sr = new StreamReader(response.ResponseStream) )
                {       
                    result = sr.ReadToEnd();
                }
            }

            return result;
        }
    }
}
