using Microsoft.VisualStudio.TestTools.UnitTesting;
using TECHIS.CloudFile;
using System.Linq;
using TECHIS.CloudFile.S3;
using System.Threading.Tasks;
using System;

namespace MSTestProject
{
    [TestClass]
    public class TestBlobManager
    {
        [TestMethod]
        public void TestWriteListDelete()
        {
            string root =  Guid.NewGuid().ToString("N");
            string path = root + "/f1";
            WriterMethods.WriteText(path, nameof(TestWriteListDelete));
            var listing1 = ListFiles(root);

            Assert.IsTrue(listing1 != null && listing1.Length > 0, "Failed to get valid location for testing delete location");
            
            ConnectedManager.Delete(listing1[0]);

            var listing2 = ListFiles(path);

            Assert.IsTrue(listing2 != null);
            Assert.IsTrue(listing2.Length < listing1.Length);
        }
        [TestMethod]
        public async Task TestWriteListDeleteAsync()
        {
            string root = Guid.NewGuid().ToString("N");
            string path = root + "/f1";
            await WriterMethods.WriteTextAsync(path, nameof(TestWriteListDelete));
            var listing1 = await ListFilesAsync(root);

            Assert.IsTrue(listing1 != null && listing1.Length > 0, "Failed to get valid location for testing delete location");

            await ConnectedManager.DeleteAsync(listing1[0]);

            var listing2 = await ListFilesAsync(path);

            Assert.IsTrue(listing2 != null);
            Assert.IsTrue(listing2.Length < listing1.Length);
        }

        [TestMethod]
        public async Task TestListContainerRootAsync()
        {
            string root = Guid.NewGuid().ToString("N");
            var paths = await WriterMethods.WriteFilesAsync(root, nameof(TestListContainerRootAsync), 2);
            
            var list = await ConnectedManager.ListAsync(null);

            Assert.IsTrue(list != null && list.Length > 0, "failed to list items in container");

            foreach (var path in paths)
            {
                await ConnectedManager.DeleteAsync(path);
            }
        }

        [TestMethod]
        public async Task TestListContainerChild()
        {
            string root1 = Guid.NewGuid().ToString("N");
            string root2 = Guid.NewGuid().ToString("N");

            int root1FileCount = 3;

            var paths = await WriterMethods.WriteFilesAsync(root1, nameof(TestListContainerRootAsync), root1FileCount);

            //Add more files
            var moreList = await WriterMethods.WriteFilesAsync(root2, nameof(TestListContainerRootAsync), 2);

            var list = await ConnectedManager.ListAsync(root1);

            Assert.IsTrue(list != null && list.Length == root1FileCount, "failed to list items in container");

            paths.AddRange(moreList);
            foreach (var path in paths)
            {
                await ConnectedManager.DeleteAsync(path);
            }
        }

        [TestMethod]
        public async Task TestListContainerSubChild()
        {
            string root1 = Guid.NewGuid().ToString("N");
            string subRoot = root1 +"/child";

            int childRootFileCount = 3;

            var moreList = await WriterMethods.WriteFilesAsync(root1, nameof(TestListContainerRootAsync), 4);

            //Add to child root
            var paths = await WriterMethods.WriteFilesAsync(subRoot, nameof(TestListContainerRootAsync), childRootFileCount);

            var list = await ConnectedManager.ListAsync(subRoot);

            Assert.IsTrue(list != null && list.Length == paths.Count, "failed to list items in container");


            paths.AddRange(moreList);
            foreach (var path in paths)
            {
                await ConnectedManager.DeleteAsync(path);
            }
        }

        private string[] ListFiles(string path)
        {
            return ConnectedManager.ListAsync(path).Result;
        }
        private async Task<string[]> ListFilesAsync(string path)
        {
            return await ConnectedManager.ListAsync(path);
        }

        private ICloudFileManager ConnectedManager => (new Manager()).Connect(WriterMethods.GetConnectionString());
        
    }
}
