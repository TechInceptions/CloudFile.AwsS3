using Amazon;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHIS.CloudFile;
using TECHIS.Core;

namespace TECHIS.CloudFile.S3
{
    public class ConnectionInfo
    {
        private const char SEPERATOR = '|';

        private AmazonS3Client _S3Client;
        private bool _UseDefaultClient = false;

        public string BucketName { get; set; }
        public string RegionName { get; set; }
        public string SecretAccessKey { get; set; }
        public string AccessKey { get; set; }
        public string ProfileName { get; set; }


        /// <summary>
        /// Get a connection info object for the S3 bucket
        /// </summary>
        /// <param name="pipeSeperatedConnectionInfo">a connection string in the following form: "BucketName|RegionName|SecretAccessKey|AccessKey|ProfileName".</param>
        public static ConnectionInfo Get(string pipeSeperatedConnectionInfo)
        {
            pipeSeperatedConnectionInfo = pipeSeperatedConnectionInfo ?? string.Empty;
            ConnectionInfo connectionInfo = new ConnectionInfo();
            connectionInfo.SetInfo(pipeSeperatedConnectionInfo);
            return connectionInfo;
        }

        //public void Connect(string pipeSeperatedConnectionInfo)
        //{
        //    InputValidator.ArgumentNullOrEmptyCheck(pipeSeperatedConnectionInfo);
        //    SetInfo(pipeSeperatedConnectionInfo);
        //    Connect();
        //}

        internal AmazonS3Client Connect()
        {
            if (_UseDefaultClient)
            {
                _S3Client = new AmazonS3Client();
            }
            else if (!(string.IsNullOrWhiteSpace(SecretAccessKey) || string.IsNullOrWhiteSpace(AccessKey) || string.IsNullOrWhiteSpace(RegionName)))
            {
                _S3Client = new AmazonS3Client(AccessKey, SecretAccessKey, RegionEndpoint.GetBySystemName(RegionName));
            }
            else if (!string.IsNullOrWhiteSpace(RegionName))
            {
                _S3Client = new AmazonS3Client(RegionEndpoint.GetBySystemName(RegionName));
            }

            return _S3Client;
        }

        private void SetInfo(string pipeSeperatedConnectionInfo)
        {
            var parts = pipeSeperatedConnectionInfo.Split(SEPERATOR);
            switch (parts.Length)
            {
                case 0:
                   _UseDefaultClient = true;
                    break;
                case 1:
                    BucketName = parts[0];
                    break;
                case 2:

                    BucketName = parts[0];
                    RegionName = parts[1];
                    break;
                case 3:
                    BucketName = parts[0];
                    RegionName = parts[1];
                    SecretAccessKey = parts[2];
                    break;
                case 4:
                    BucketName = parts[0];
                    RegionName = parts[1];
                    SecretAccessKey = parts[2];
                    AccessKey = parts[3];
                    break;
                case 5:
                   BucketName       = parts[0];
                   RegionName       = parts[1];
                   SecretAccessKey  = parts[2];
                   AccessKey        = parts[3];
                   ProfileName      = parts[4];
                    break;
                default:
                    break;
            }
            
        }
    }
}
