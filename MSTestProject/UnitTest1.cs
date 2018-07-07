using Microsoft.VisualStudio.TestTools.UnitTesting;
using TECHIS.CloudFile;
using System.Linq;
using TECHIS.CloudFile.S3;

namespace MSTestProject
{
    [TestClass]
    public class TestBlobManager
    {
        [TestMethod]
        public void TestDelete()
        {
            string path = "ForDelete";
            var list = ListFilesAsync(path);

            Assert.IsTrue(list != null && list.Length > 0, "Failed to get valid location for testing delete location");

            var initial1st = list[0];
            ConnectedManager.DeleteAsync(initial1st).Wait();

            var new1st = ListFilesAsync(path)[0];

            Assert.IsFalse(string.Equals(initial1st, new1st), "Failed to delete item");
        }

        [TestMethod]
        public void TestListContainerRoot()
        {
            string path = null;
            var list = ConnectedManager.ListAsync(path).Result;

            Assert.IsTrue(list != null && list.Length > 0, "failed to list items in container");
        }

        [TestMethod]
        public void TestListContainerChild()
        {
            string path = "PackageRepo";
            string[] list = ConnectedManager.ListAsync(path).Result;

            Assert.IsTrue(list != null && list.Length > 0 && list.All(p => p.Contains($"{path}/")), "failed to list only items in child folder");
        }

        [TestMethod]
        public void TestListContainerSubChild()
        {
            string path = "Samples/graphics/graphics";
            var list = ConnectedManager.ListAsync(path).Result;

            Assert.IsTrue(list != null && list.Length > 0 && list.All(p => p.Contains($"{path}/")), "failed to list only items in child folder");
        }

        private string[] ListFilesAsync(string path)
        {
            return ConnectedManager.ListAsync(path).Result;
        }

        private ICloudFileManager ConnectedManager => (new Manager()).Connect(GetConnectionInfo());


        private static string GetConnectionInfo()
        {//techis-s3-test
            //BucketName|RegionName|SecretAccessKey|AccessKey|ProfileName
            //
            return "";
        }
    }
}
