using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHIS.CloudFile;
using TECHIS.Core;

namespace TECHIS.CloudFile.AwsS3
{
    public abstract class S3Access:IDisposable
    {
        #region Fields 
        private Amazon.S3.AmazonS3Client _S3Client;
        private string _Key;
        private string _Secret;

        #endregion

        #region Properties 


        public virtual Encoding Encoding { get; protected set; }

        public bool IsClientValid { get; }
        public bool IsValidBucket { get; protected set; }

        public abstract string KeySecretSeperator
        { get; }

        public string BucketName { get; protected set; }

        #endregion

        #region Publid Methods 
        /// <summary>
        /// Sets up an Amazon.S3.AmazonS3Client instance
        /// </summary>
        /// <param name="secretKeyAccessKeyBucketName">A comma seperated, 3 part string. 
        /// Example: "SecretKey, AccessKey, BucketName" </param>
        public void Connect(string secretKeyAccessKeyBucketName, Encoding encoding = null)
        {
            InputValidator.ArgumentNullOrEmptyCheck(secretKeyAccessKeyBucketName, nameof(secretKeyAccessKeyBucketName));
            var parts = secretKeyAccessKeyBucketName.Split(new[] { KeySecretSeperator }, StringSplitOptions.None);

            if (parts.Length<3)
            {
                throw new ArgumentException(Errors.ConnectionStringInvalid, secretKeyAccessKeyBucketName);
            }

            Connect(parts[1], parts[2], parts[0], encoding);
        }
        /// <summary>
        /// Sets up an Amazon.S3.AmazonS3Client instance
        /// </summary>
        /// <param name="accessKeyBucketName">A comma seperated, 2 part string. 
        /// Example: "AccessKey, BucketName" </param>
        /// <param name="secret">The secret key</param>
        public void Connect(string accessKeyBucketName, string secret, Encoding encoding = null)
        {
            InputValidator.ArgumentNullOrEmptyCheck(accessKeyBucketName, nameof(accessKeyBucketName));
            InputValidator.ArgumentNullOrEmptyCheck(secret, nameof(secret));

            var parts = accessKeyBucketName.Split(new[] { KeySecretSeperator }, StringSplitOptions.None);
            if (parts.Length<2)
            {
                throw new ArgumentException(Errors.ConnectionStringInvalid, accessKeyBucketName);
            }

            Connect(parts[0], parts[1], secret, encoding);
        }
        #endregion

        #region Private Methods 
        private void Connect(string accessKey, string bucketName, string secret, Encoding encoding)
        {
            InputValidator.ArgumentNullOrEmptyCheck(accessKey,  nameof(accessKey));
            InputValidator.ArgumentNullOrEmptyCheck(bucketName, nameof(bucketName));
            InputValidator.ArgumentNullOrEmptyCheck(secret,     nameof(secret));

            _S3Client = new Amazon.S3.AmazonS3Client(accessKey, secret);

            if (encoding != null)
            {
                Encoding = encoding;
            }

            _Key = accessKey;
            _Secret = secret;
            BucketName = bucketName;
        }
        #endregion

        protected virtual void Reset()
        {
            _S3Client?.Dispose();
            _S3Client = new Amazon.S3.AmazonS3Client(_Key, _Secret);
        }

        public void Dispose()
        {
            ((IDisposable)_S3Client)?.Dispose();
        }
    }
}
