using Elect.Data.IO.FileUtils;
using FluentValidation;
using Goblin.Resource.Share.Models;

namespace Goblin.Resource.Core.Validators
{
    public class UploadFileModelValidator : AbstractValidator<GoblinResourceUploadFileModel>
    {
        public UploadFileModelValidator()
        {
            RuleFor(x => x.ContentBase64)
                .Must(FileHelper.IsValidBase64)
                .WithMessage("The Content must be in Valid Base64 format");
            
            RuleFor(x => x.ImageMaxWidthPx)
                .Must(x => !x.HasValue ||  x <= SystemSetting.Current.ImageMaxWidthPx)
                .WithMessage($"Maximum Image Width is {SystemSetting.Current.ImageMaxWidthPx}px");
            
            RuleFor(x => x.ImageMaxHeightPx)
                .Must(x => !x.HasValue ||  x <= SystemSetting.Current.ImageMaxHeightPx)
                .WithMessage($"Maximum Image Width is {SystemSetting.Current.ImageMaxHeightPx}px");
        }
        
        public string Name { get; set; }

        public string ContentBase64 { get; set; }

        /// <summary>
        ///     Can pass multiple folder by "/" <br />
        ///     E.g Identity/Avatar
        /// </summary>
        public string Folder { get; set; }

        public bool IsEnableCompressImage { get; set; } = true;

        public int? ImageMaxWidthPx { get; set; }

        public int? ImageMaxHeightPx { get; set; }
    }
}