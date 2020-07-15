using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Elect.Web.HttpUtils;
using Elect.Web.Swagger.Attributes;
using Goblin.Resource.Contract.Service;
using Goblin.Resource.Core;
using Goblin.Resource.Share;
using Goblin.Resource.Share.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Goblin.Resource.Controllers
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
        ///     Example: cngxhiuqkkq-f-c0-w0-h0-sample-excel.xlsx <br />
        ///     <br />
        ///     <b>Slug Format for Image</b>: {id}-i-{hex color}-{width in px}-{height in px}-{file name}.{extension} <br />
        ///     Example: cngxhiuqkkq-i-c2b181b-w288-h163-sample-image.png <br />
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
        [Route(GoblinResourceEndpoints.UploadFile)]
        [SwaggerResponse(StatusCodes.Status201Created, "File Saved", typeof(GoblinResourceFileModel))]
        public async Task<IActionResult> Upload([FromBody] GoblinResourceUploadFileModel model, CancellationToken cancellationToken = default)
        {
            var fileModel = await _fileService.SaveAsync(model, cancellationToken);

            fileModel.Slug = Path.Combine(HttpContext.Request.GetDomain(), SystemSetting.Current.ResourceFolderEndpoint.Trim('/'), fileModel.Slug);
            
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
        [Route(GoblinResourceEndpoints.GetFile)]
        [SwaggerResponse(StatusCodes.Status200OK, "File Information", typeof(GoblinResourceFileModel))]
        public async Task<IActionResult> Get([FromQuery] string slug, CancellationToken cancellationToken = default)
        {
            var fileModel = await _fileService.GetAsync(slug, cancellationToken);

            fileModel.Slug = Path.Combine(HttpContext.Request.GetDomain(), SystemSetting.Current.ResourceFolderEndpoint.Trim('/'), fileModel.Slug);

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
        [Route(GoblinResourceEndpoints.DeleteFile)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "File Deleted")]
        public async Task<IActionResult> Delete([FromQuery] string slug, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return BadRequest("Invalid Slug");
            }
            
            var resourcePath = Path.Combine(HttpContext.Request.GetDomain(), SystemSetting.Current.ResourceFolderEndpoint.Trim('/'));

            slug = slug.Replace(resourcePath, string.Empty);

            await _fileService.DeleteAsync(slug, cancellationToken);
            
            return NoContent();
        }
    }
}