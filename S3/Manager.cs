using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TECHIS.CloudFile.S3
{
    public class Manager : ICloudFileManager, ICloudFileManagerFactory
    {
        private const string MARKER_REQUEST = "NextMarker:";

        private AmazonS3Client _S3Client;
        private ConnectionInfo _ConnectionInfo;
        public ICloudFileManager Connect(string pipeSeperatedConnectionInfo, Encoding encoding = null)
        {
            _S3Client = (_ConnectionInfo = ConnectionInfo.Get(pipeSeperatedConnectionInfo)).Connect();
            return this;
        }

        public ICloudFileManager Connect(string pipeSeperatedConnectionInfo, string connectionDetails, Encoding encoding = null)
        {
            _S3Client = (_ConnectionInfo = ConnectionInfo.Get(pipeSeperatedConnectionInfo)).Connect();
            return this;
        }

        public void Delete(string fileName)
        {
            var task = DeleteAsync(fileName);
            task.Wait();
        }

        public async Task DeleteAsync(string fileName)
        {
            DeleteObjectRequest request = new DeleteObjectRequest()
            {
                BucketName = _ConnectionInfo.BucketName,
                Key = fileName
            };

            var response = await _S3Client.DeleteObjectAsync(request).ConfigureAwait(false);
        }

        public string[] List(string containerPath)
        {
            return ListAsync(containerPath).Result;
        }

        public async Task<string[]> ListAsync(string containerPath)
        {
            ListObjectsRequest request = new ListObjectsRequest
            {
                BucketName = _ConnectionInfo.BucketName
            };

            ListObjectsResponse response = null;
            if (string.IsNullOrWhiteSpace(containerPath))
            {
                response = await _S3Client.ListObjectsAsync(request).ConfigureAwait(false);
            }
            else if (containerPath.StartsWith(MARKER_REQUEST))
            {
                // list only things that come after "bar" alphabetically
                var initialMarker = containerPath.Replace(MARKER_REQUEST, "");

                request.Prefix = null;
                request.Marker = initialMarker;
                response = await _S3Client.ListObjectsAsync(request).ConfigureAwait(false);
            }
            else if(! string.IsNullOrWhiteSpace(containerPath))
            {
                // list only things starting with "foo"
                request.Prefix = containerPath;
                response = await _S3Client.ListObjectsAsync(request).ConfigureAwait(false);
            }

            List<string> filePaths = new List<string>();
            foreach (S3Object entry in response.S3Objects)
            {
                filePaths.Add(entry.Key);
            }

            return filePaths.ToArray();
        }
    }
}
