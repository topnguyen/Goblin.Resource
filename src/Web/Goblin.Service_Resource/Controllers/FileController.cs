using System.Threading;
using System.Threading.Tasks;
using Elect.Web.Swagger.Attributes;
using Goblin.Service_Resource.Contract.Service;
using Goblin.Service_Resource.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Goblin.Service_Resource.Controllers
{
    public class FileController : BaseController
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        ///     Upload File
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("File")]
        [HttpPost]
        [Route("/files")]
        public async Task<IActionResult> Upload([FromBody] UploadFileModel model, CancellationToken cancellationToken = default)
        {
            var fileModel = await _fileService.SaveAsync(model, cancellationToken);
            
            return Created(fileModel.Slug, fileModel);
        }
    }
}