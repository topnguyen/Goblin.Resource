using System.Threading;
using System.Threading.Tasks;
using Elect.Web.Swagger.Attributes;
using Goblin.Service_Resource.Contract.Service;
using Goblin.Service_Resource.Share;
using Goblin.Service_Resource.Share.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        /// <remarks>
        ///     <b>Slug Format for File</b>: {id}-f-{file name}.{extension} <br />
        ///     Example: cngxhiuqkkq-f-sample-excel.xlsx <br />
        ///     <br />
        ///     <b>Slug Format for Image</b>: {id}-i-{hex color}-{width in px}-{height in px}-{file name}.{extension} <br />
        ///     Example: cngxhiuqkkq-i-#2B181B-w288-h163-sample-image.png <br />
        ///     <br />
        ///     <b>Image Slug</b><br />
        ///     "{image slug}-s" to access Skeleton/Small 60x60 image <br />
        ///     "{image slug}-t" to access Thumbnail 600x600 image
        /// </remarks>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("File")]
        [HttpPost]
        [Route(Endpoints.UploadFile)]
        [SwaggerResponse(StatusCodes.Status201Created, "File Saved", typeof(FileModel))]
        public async Task<IActionResult> Upload([FromBody] UploadFileModel model, CancellationToken cancellationToken = default)
        {
            var fileModel = await _fileService.SaveAsync(model, cancellationToken);
            
            return Created(fileModel.Slug, fileModel);
        }
        
        /// <summary>
        ///     Get File
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("File")]
        [HttpGet]
        [Route(Endpoints.GetFile)]
        [SwaggerResponse(StatusCodes.Status200OK, "File Information", typeof(FileModel))]
        public async Task<IActionResult> Get([FromQuery] string slug, CancellationToken cancellationToken = default)
        {
            var fileModel = await _fileService.GetAsync(slug, cancellationToken);
            
            return Ok(fileModel);
        }
        
        /// <summary>
        ///     Delete File
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("File")]
        [HttpDelete]
        [Route(Endpoints.DeleteFile)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "File Deleted")]
        public async Task<IActionResult> Delete([FromQuery] string slug, CancellationToken cancellationToken = default)
        {
            await _fileService.DeleteAsync(slug, cancellationToken);
            
            return NoContent();
        }
    }
}