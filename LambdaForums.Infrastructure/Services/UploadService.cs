using LambdaForums.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LambdaForums.Infrastructure.Services
{
    public class UploadService : IUpload
    {
        public IConfiguration Configuration;

        public UploadService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public CloudBlobContainer GetBlobContainer(string connectionString, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(containerName);
        }
    }
}
