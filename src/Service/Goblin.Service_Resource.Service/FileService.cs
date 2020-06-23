using System;
using System.Linq;
using Elect.DI.Attributes;
using Goblin.Service_Resource.Contract.Repository.Interfaces;
using Goblin.Service_Resource.Contract.Service;
using System.Threading;
using System.Threading.Tasks;
using Elect.Core.SecurityUtils;
using Elect.Data.IO.ImageUtils;
using Elect.Mapper.AutoMapper.ObjUtils;
using Elect.Web.StringUtils;
using Goblin.Service_Resource.Contract.Repository.Models;
using Goblin.Service_Resource.Core;
using Goblin.Service_Resource.Share.Models;

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

            FileServiceHelper.Correct(model);

            // File Checksum
            
            var fileHash = SecurityHelper.EncryptSha256(model.ContentBase64);

            var fileEntity = _fileRepo.Get(x => x.Hash == fileHash).FirstOrDefault();

            // Return if Found in Database
            
            if (fileEntity != null)
            {
                return fileEntity.MapTo<FileModel>();
            }
            
            fileEntity = model.MapTo<FileEntity>();

            fileEntity.Hash = fileHash;
            
            var fileBytes = Convert.FromBase64String(model.ContentBase64);

            var imageInfo = ImageHelper.GetImageInfo(fileBytes);

            // Fill information based on File is Image or not

            FileServiceHelper.FillInformation(imageInfo, fileEntity);

            var fileName = FileServiceHelper.GenerateId();

            cancellationToken.ThrowIfCancellationRequested();

            // Save Files

            if (imageInfo != null)
            {
                // Main Image

                var isNeedResizeImage = model.ImageMaxHeightPx < fileEntity.ImageHeightPx ||
                                        model.ImageMaxWidthPx < fileEntity.ImageWidthPx;

                fileBytes = FileServiceHelper.ResizeAndCompressImage(fileBytes,
                    isNeedResizeImage,
                    model.ImageMaxWidthPx.Value,
                    model.ImageMaxHeightPx.Value,
                    model.IsEnableCompressImage);

                fileEntity.IsCompressedImage = true;

                // Refill information after resize and compress

                if (isNeedResizeImage || model.IsEnableCompressImage)
                {
                    var newImageInfo = ImageHelper.GetImageInfo(fileBytes);
                    FileServiceHelper.FillInformation(newImageInfo, fileEntity);
                }
                
                fileName += $"-i-{fileEntity.ImageDominantHexColor}-w{fileEntity.ImageWidthPx}-h{fileEntity.ImageHeightPx}-{fileEntity.Name.ToFriendlySlug()}";

                // Save File
                
                fileEntity.Slug = FileServiceHelper.SaveFile(fileBytes, model.Folder, fileName, string.Empty, fileEntity.Extension);

                fileEntity.ContentLength = fileBytes.Length;
                
                // --------------------------
                //    Save More Image Size
                // --------------------------

                // Image Skeleton

                var imageSkeletonBytes = FileServiceHelper.ResizeAndCompressImage(fileBytes,
                    true,
                    SystemSetting.Current.ImageSkeletonMaxWidthPx,
                    SystemSetting.Current.ImageSkeletonMaxHeightPx,
                    true);
                
                FileServiceHelper.SaveFile(imageSkeletonBytes, model.Folder, fileName, "-s", fileEntity.Extension);

                // Image Thumbnail

                var imageThumbnailBytes = FileServiceHelper.ResizeAndCompressImage(fileBytes,
                    true,
                    SystemSetting.Current.ImageThumbnailMaxWidthPx,
                    SystemSetting.Current.ImageThumbnailMaxHeightPx,
                    true);

                FileServiceHelper.SaveFile(imageThumbnailBytes, model.Folder, fileName, "-t", fileEntity.Extension);
            }
            else
            {
                fileName += $"-f-{fileEntity.Name.ToFriendlySlug()}";
                
                fileEntity.Slug = FileServiceHelper.SaveFile(fileBytes, model.Folder, fileName, string.Empty, fileEntity.Extension);

                fileEntity.ContentLength = fileBytes.Length;
            }
            
            _fileRepo.Add(fileEntity);

            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            // Response
            
            var fileModel = fileEntity.MapTo<FileModel>();

            return fileModel;
        }
    }
}