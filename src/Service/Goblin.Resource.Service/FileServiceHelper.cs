using System;
using System.IO;
using System.Linq;
using Elect.Core.StringUtils;
using Elect.Data.IO;
using Elect.Data.IO.DirectoryUtils;
using Elect.Data.IO.FileUtils;
using Elect.Data.IO.ImageUtils;
using Elect.Data.IO.ImageUtils.CompressUtils;
using Elect.Data.IO.ImageUtils.Models;
using Elect.Data.IO.ImageUtils.ResizeUtils;
using Elect.Web.StringUtils;
using Goblin.Resource.Contract.Repository.Models;
using Goblin.Resource.Core;
using Goblin.Resource.Share.Models;

namespace Goblin.Resource.Service
{
    public static class FileServiceHelper
    {
        /// <summary>
        ///     Pre-handle the model before proceed
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static void Correct(GoblinResourceUploadFileModel model)
        {
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
        }

        /// <summary>
        ///     Fill more information based on File is Image or not
        /// </summary>
        /// <param name="imageInfo"></param>
        /// <param name="fileEntity"></param>
        public static void FillInformation(ImageModel imageInfo, FileEntity fileEntity)
        {
            fileEntity.IsImage = false;
            
            fileEntity.IsCompressedImage = false;

            fileEntity.Extension = Path.GetExtension(fileEntity.Name);

            fileEntity.MimeType =
                string.IsNullOrWhiteSpace(fileEntity.Extension)
                    ? "application/octet-stream"
                    : MimeTypeHelper.GetMimeType(fileEntity.Extension);
            
            if (imageInfo != null)
            {
                // Image File

                fileEntity.IsImage = true;

                fileEntity.ImageDominantHexColor = imageInfo.DominantHexColor.ToLowerInvariant();
                
                fileEntity.ImageWidthPx = imageInfo.WidthPx;
                
                fileEntity.ImageHeightPx = imageInfo.HeightPx;

                fileEntity.MimeType = imageInfo.MimeType;
                
                fileEntity.Extension = imageInfo.Extension;
            }

            // Handle file name

            if (string.IsNullOrWhiteSpace(fileEntity.Name))
            {
                fileEntity.Name = FileHelper.MakeValidFileName(fileEntity.Name);
                
                if (fileEntity.Name.EndsWith(fileEntity.Extension))
                {
                    // Get File Name Only

                    fileEntity.Name = fileEntity.Name.Substring(0, fileEntity.Name.Length - fileEntity.Extension.Length);
                }
            }
        }

        public static string GenerateId()
        {
            var slugId = IdHelper.ToString(ulong.Parse(DateTime.UtcNow.Ticks.ToString())).ToFriendlySlug();

            return slugId;
        }
        
        public static string GenerateFileName(string originalFileName, ImageModel imageModel)
        {
            var fileName = GenerateId();

            if (imageModel != null)
            {
                fileName += $"-i-c{imageModel.DominantHexColor}-w{imageModel}";
            }
            else
            {
                fileName += "-f-c0-w0-h0";
            }

            return fileName;
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
        public static string SaveFile(byte[] fileBytes, 
            string folderName,
            string fileName,
            string fileNamePostfix,
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
        public static string SaveFile(byte[] fileBytes, string fileRelativePath)
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

        public static byte[] ResizeAndCompressImage(byte[] imageFileBytes,
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