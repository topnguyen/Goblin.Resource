using System;
using System.IO;
using System.Linq;
using Elect.DI.Attributes;
using Goblin.Service_Resource.Contract.Repository.Interfaces;
using Goblin.Service_Resource.Contract.Service;
using System.Threading;
using System.Threading.Tasks;
using Elect.Core.SecurityUtils;
using Elect.Core.StringUtils;
using Elect.Data.IO;
using Elect.Data.IO.DirectoryUtils;
using Elect.Data.IO.FileUtils;
using Elect.Data.IO.ImageUtils;
using Elect.Data.IO.ImageUtils.CompressUtils;
using Elect.Data.IO.ImageUtils.ResizeUtils;
using Elect.Mapper.AutoMapper.ObjUtils;
using Elect.Web.StringUtils;
using Goblin.Service_Resource.Contract.Repository.Models;
using Goblin.Service_Resource.Core;
using Goblin.Service_Resource.Core.Models;

namespace Goblin.Service_Resource.Service
{
    [ScopedDependency(ServiceType = typeof(IFileService))]
    public class FileService : Base.Service, IFileService
    {
        private readonly IRepository<FileEntity> _fileRepo;

        public FileService(IUnitOfWork unitOfWork, IRepository<FileEntity> fileRepo) : base(unitOfWork)
        {
            _fileRepo = fileRepo;
        }

        public async Task<FileModel> SaveAsync(UploadFileModel model, CancellationToken cancellationToken = default)
        {
            // Handle Parameters

            if (string.IsNullOrWhiteSpace(model.Folder))
            {
                model.Folder = SystemSetting.Current.DefaultFolderName;
            }

            if (!string.IsNullOrWhiteSpace(model.Folder))
            {
                var folders = model.Folder
                    .Split("/")
                    .Select(x => x.Trim().ToLowerInvariant())
                    .ToList();

                model.Folder = string.Join("/", folders);
            }

            model.Name = model.Name?.Trim()
                .Replace("-s", "~s")
                .Replace("-m", "~m");

            model.ContentBase64 = ImageHelper.GetBase64Format(model.ContentBase64);

            if (model.ImageMaxHeightPx.HasValue && model.ImageMaxHeightPx > 0)
            {
                model.ImageMaxHeightPx =
                    model.ImageMaxHeightPx > SystemSetting.Current.ImageMaxHeightPx
                        ? SystemSetting.Current.ImageMaxHeightPx
                        : model.ImageMaxHeightPx;
            }
            else
            {
                model.ImageMaxHeightPx = SystemSetting.Current.ImageMaxHeightPx;
            }

            if (model.ImageMaxWidthPx.HasValue && model.ImageMaxWidthPx > 0)
            {
                model.ImageMaxWidthPx =
                    model.ImageMaxHeightPx > SystemSetting.Current.ImageMaxWidthPx
                        ? SystemSetting.Current.ImageMaxWidthPx
                        : model.ImageMaxWidthPx;
            }
            else
            {
                model.ImageMaxWidthPx = SystemSetting.Current.ImageMaxWidthPx;
            }

            // File Checksum
            var fileHash = SecurityHelper.EncryptSha256(model.ContentBase64);

            var fileEntity = _fileRepo.Get(x => x.Hash == fileHash).FirstOrDefault();

            if (fileEntity != null)
            {
                return fileEntity.MapTo<FileModel>();
            }
            
            // Map to Entity
            fileEntity = model.MapTo<FileEntity>();

            var fileBytes = Convert.FromBase64String(model.ContentBase64);

            var imageInfo = ImageHelper.GetImageInfo(fileBytes);

            if (imageInfo != null)
            {
                // Image File

                fileEntity.IsImage = true;
                fileEntity.IsCompressedImage = model.IsEnableCompressImage;

                fileEntity.ImageDominantHexColor = imageInfo.DominantHexColor;
                fileEntity.ImageWidthPx = imageInfo.WidthPx;
                fileEntity.ImageHeightPx = imageInfo.HeightPx;

                fileEntity.MimeType = imageInfo.MimeType;
                fileEntity.Extension = imageInfo.Extension;
            }
            else
            {
                // Document File

                fileEntity.IsImage = false;
                fileEntity.IsCompressedImage = false;

                fileEntity.Extension = Path.GetExtension(fileEntity.Name);
                fileEntity.MimeType = string.IsNullOrWhiteSpace(fileEntity.Extension)
                    ? "application/octet-stream"
                    : MimeTypeHelper.GetMimeType(fileEntity.Extension);
            }

            if (fileEntity.Name?.EndsWith(fileEntity.Extension) == true)
            {
                // Get File Name Only

                fileEntity.Name = fileEntity.Name.Substring(0, fileEntity.Name.Length - fileEntity.Extension.Length);
            }

            // Generate File Slug/Name

            var slugId = IdHelper.ToString(ulong.Parse(DateTime.UtcNow.Ticks.ToString())).ToFriendlySlug();

            var fileName = slugId;

            if (!string.IsNullOrWhiteSpace(fileEntity.Name))
            {
                fileName += $"-{fileEntity.Name.ToFriendlySlug()}";
            }

            // Save File

            cancellationToken.ThrowIfCancellationRequested();

            // Save Pre-resize Images

            if (imageInfo != null)
            {
                // Image Skeleton

                var imageSkeletonBytes = ResizeAndCompressImage(fileBytes,
                    true,
                    SystemSetting.Current.ImageSkeletonMaxWidthPx,
                    SystemSetting.Current.ImageSkeletonMaxHeightPx,
                    true);

                SaveFile(imageSkeletonBytes, model.Folder, fileName, "-s", fileEntity.Extension);

                // Image Thumbnail

                var imageThumbnailBytes = ResizeAndCompressImage(fileBytes,
                    true,
                    SystemSetting.Current.ImageThumbnailMaxWidthPx,
                    SystemSetting.Current.ImageThumbnailMaxHeightPx,
                    true);

                SaveFile(imageThumbnailBytes, model.Folder, fileName, "-t", fileEntity.Extension);

                // Image

                var isNeedResizeImage = model.ImageMaxHeightPx < fileEntity.ImageHeightPx ||
                                        model.ImageMaxWidthPx < fileEntity.ImageWidthPx;

                fileBytes = ResizeAndCompressImage(fileBytes,
                    isNeedResizeImage,
                    model.ImageMaxWidthPx.Value,
                    model.ImageMaxHeightPx.Value,
                    model.IsEnableCompressImage);

                fileEntity.Slug = SaveFile(fileBytes, model.Folder, fileName, string.Empty, fileEntity.Extension);

                fileEntity.ContentLength = fileBytes.Length;
            }
            else
            {
                fileEntity.Slug = SaveFile(fileBytes, model.Folder, fileName, string.Empty, fileEntity.Extension);

                fileEntity.ContentLength = fileBytes.Length;
            }

            _fileRepo.Add(fileEntity);

            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            var fileModel = fileEntity.MapTo<FileModel>();

            return fileModel;
        }

        /// <summary>
        ///     Save File and Return Slug
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <param name="fileNamePostfix"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        private static string SaveFile(byte[] fileBytes, string folderName, string fileName, string fileNamePostfix,
            string fileExtension)
        {
            var fileRelativePath = $"{folderName}/{fileName}{fileNamePostfix}{fileExtension}".Trim('/');

            return SaveFile(fileBytes, fileRelativePath);
        }

        /// <summary>
        ///     Save File and Return Slug
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="fileRelativePath"></param>
        private static string SaveFile(byte[] fileBytes, string fileRelativePath)
        {
            fileRelativePath = PathHelper.CorrectPathSeparatorChar(fileRelativePath);

            var fileAbsolutePath = Path.Combine(Directory.GetCurrentDirectory(),
                SystemSetting.Current.ResourceFolderPath,
                fileRelativePath);

            var folderAbsolutePath = Path.GetDirectoryName(fileAbsolutePath);

            DirectoryHelper.CreateIfNotExist(folderAbsolutePath);

            File.WriteAllBytes(fileAbsolutePath, fileBytes);

            var fileSlug = fileRelativePath.Replace(Path.DirectorySeparatorChar, '/');

            return fileSlug;
        }

        private static byte[] ResizeAndCompressImage(byte[] imageFileBytes,
            bool isResize,
            int maxWidthPx,
            int maxHeightPx,
            bool isCompress)
        {
            var newImageFileBytes = imageFileBytes;

            if (isResize)
            {
                newImageFileBytes = ImageResizeHelper.Resize(imageFileBytes, maxWidthPx, maxHeightPx);
            }

            if (!isCompress)
            {
                return newImageFileBytes;
            }

            using var streamToCompress = new MemoryStream(newImageFileBytes);

            var compressResult = ImageCompressor.Compress(streamToCompress);

            newImageFileBytes = compressResult.ResultFileStream.ToArray();

            return newImageFileBytes;
        }
    }
}