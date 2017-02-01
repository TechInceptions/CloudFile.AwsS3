namespace TECHIS.CloudFile.AwsS3
{
    internal class Errors
    {
        internal const string ConnectionStringInvalid = "The connection string must contain two parts: the key and the secret";
        internal const string ServiceClientObjectInvalid = "The client object is invalid. Did you forget to call connect?";
    }
}