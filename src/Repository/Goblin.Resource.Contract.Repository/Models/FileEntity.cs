namespace Goblin.Resource.Contract.Repository.Models
{
    public class FileEntity : GoblinEntity
    {
        public string Slug { get; set; }

        public string Folder { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public string MimeType { get; set; }

        public long ContentLength { get; set; }
        
        // Hash to prevent duplicate
        
        public string Hash { get; set; }

        // Image
        
        public bool IsImage { get; set; }
        
        public bool IsCompressedImage { get; set; } 

        public string ImageDominantHexColor { get; set; }

        public int ImageWidthPx { get; set; } = -1;

        public int ImageHeightPx { get; set; } = -1;
    }
}