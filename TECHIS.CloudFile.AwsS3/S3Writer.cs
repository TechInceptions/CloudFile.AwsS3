using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Model;

namespace TECHIS.CloudFile.AwsS3
{
    public class S3Writer : S3Access, ICloudFileWriterFactory,ICloudFileWriter
    {
        
        public override char[] KeySeperator
        {
            get
            {
                return GlobalConstants.KeySeperator;
            }
        }

        public void WriteToBlob(byte[] data, string fileName)
        {
            if (!IsClientValid)
            {
                throw new InvalidOperationException(Errors.ServiceClientObjectInvalid);
            }

            PutObjectRequest request = new PutObjectRequest();
            using (MemoryStream ms = new MemoryStream(data))
            {
                request.BucketName = BucketName;
                request.Key = fileName;
                request.InputStream = ms;
                S3Client.PutObject(request);
            }
        }

        public void WriteToBlob(Stream ms, string fileName)
        {
            if (!IsClientValid)
            {
                throw new InvalidOperationException(Errors.ServiceClientObjectInvalid);
            }

            PutObjectRequest request = new PutObjectRequest();

            request.BucketName = BucketName;
            request.Key = fileName;
            request.InputStream = ms;
            S3Client.PutObject(request);

        }

        public async Task WriteToBlobAsync(byte[] data, string fileName)
        {
            if (!IsClientValid)
            {
                throw new InvalidOperationException(Errors.ServiceClientObjectInvalid);
            }

            PutObjectRequest request = new PutObjectRequest();
            using (MemoryStream ms = new MemoryStream(data))
            {
                request.BucketName = BucketName;
                request.Key = fileName;
                request.InputStream = ms;
                await S3Client.PutObjectAsync(request);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms">The stream to read from. The stream will be read from the current position. 
        /// After data is uploaded, the stream will NOT be closed.</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task WriteToBlobAsync(Stream ms, string fileName)
        {
            if (!IsClientValid)
            {
                throw new InvalidOperationException(Errors.ServiceClientObjectInvalid);
            }
            PutObjectRequest request = new PutObjectRequest();

            request.AutoCloseStream = false;
            request.AutoResetStreamPosition = false;

            request.BucketName = BucketName;
            request.Key = fileName;
            request.InputStream = ms;
            await S3Client.PutObjectAsync(request);
        }

        /// <summary>
        /// Sets up an Amazon.S3.AmazonS3Client instance
        /// </summary>
        /// <param name="secretKeyAccessKeyBucketName">A comma seperated, 3 part string. 
        /// Example: "SecretKey, AccessKey, BucketName" </param>
        public new ICloudFileWriter Connect(string secretKeyAccessKeyBucketName, Encoding encoding)
        {
            base.Connect(secretKeyAccessKeyBucketName, encoding);
            return this;
        }

        /// <summary>
        /// Sets up an Amazon.S3.AmazonS3Client instance
        /// </summary>
        /// <param name="accessKeyBucketName">A comma seperated, 2 part string. 
        /// Example: "AccessKey, BucketName" </param>
        /// <param name="secret">The secret key</param>
        public new ICloudFileWriter Connect(string accessKeyBucketName, string secret, Encoding encoding)
        {
            base.Connect(accessKeyBucketName, secret, encoding);
            return this;
        }
    }
}
