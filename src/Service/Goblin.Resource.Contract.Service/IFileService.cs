using System.Threading;
using System.Threading.Tasks;
using Goblin.Resource.Share.Models;

namespace Goblin.Resource.Contract.Service
{
    public interface IFileService
    {
        Task<GoblinResourceFileModel> SaveAsync(GoblinResourceUploadFileModel model, CancellationToken cancellationToken = default);
        
        Task<GoblinResourceFileModel> GetAsync(string slug, CancellationToken cancellationToken = default);
        
        Task DeleteAsync(string slug, CancellationToken cancellationToken = default);
    }
}