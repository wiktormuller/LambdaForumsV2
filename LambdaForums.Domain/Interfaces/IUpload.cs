using Microsoft.WindowsAzure.Storage.Blob;

namespace LambdaForums.Domain.Interfaces
{
    public interface IUpload
    {
        CloudBlobContainer GetBlobContainer(string connectionString, string containerName);
    }
}
