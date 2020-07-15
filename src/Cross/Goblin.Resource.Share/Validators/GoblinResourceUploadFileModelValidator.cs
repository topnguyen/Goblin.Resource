using Elect.Data.IO.FileUtils;
using FluentValidation;
using Goblin.Resource.Share.Models;

namespace Goblin.Resource.Share.Validators
{
    public class GoblinResourceUploadFileModelValidator : AbstractValidator<GoblinResourceUploadFileModel>
    {
        public GoblinResourceUploadFileModelValidator()
        {
            RuleFor(x => x.ContentBase64)
                .Must(FileHelper.IsValidBase64)
                .WithMessage("The Content must be in Valid Base64 format");
            
            RuleFor(x => x.ImageMaxWidthPx)
                .Must(x => !x.HasValue ||  x <= 2560)
                .WithMessage($"Maximum Image Width is 2560px");
            
            RuleFor(x => x.ImageMaxHeightPx)
                .Must(x => !x.HasValue ||  x <= 1440)
                .WithMessage($"Maximum Image Height is 1440px");
        }
    }
}