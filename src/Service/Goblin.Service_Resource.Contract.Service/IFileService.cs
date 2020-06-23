using System.Threading;
using System.Threading.Tasks;
using Goblin.Service_Resource.Share.Models;

namespace Goblin.Service_Resource.Contract.Service
{
    public interface IFileService
    {
        Task<FileModel> SaveAsync(UploadFileModel model, CancellationToken cancellationToken = default);
    }
}