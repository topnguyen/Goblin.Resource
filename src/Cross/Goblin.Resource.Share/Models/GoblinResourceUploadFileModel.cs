using System.ComponentModel.DataAnnotations;
using Goblin.Core.Models;

namespace Goblin.Resource.Share.Models
{
    public class GoblinResourceUploadFileModel : GoblinApiSubmitRequestModel
    {
        public string Name { get; set; }

        [Required]
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