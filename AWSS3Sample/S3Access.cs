using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHIS.CloudFile;

namespace AWSS3Sample
{
    public class S3Access
    {
        private Amazon.S3.AmazonS3Client _S3Client;

        public void Connect(string containerUri, Encoding encoding = null)
        {
            throw new NotImplementedException();
        }

        public void Connect(string azureStorageConnectionString, string containerName, Encoding encoding = null)
        {
            throw new NotImplementedException();
        }
    }
}
