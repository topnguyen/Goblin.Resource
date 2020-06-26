namespace Goblin.Resource.Core
{
    public class SystemSetting
    {
        public static SystemSetting Current { get; set; }

        /// <summary>
        ///     Authorization Key to Access the Application
        /// </summary>
        /// <remarks>Use for protect Service in Public Network, empty or null for allow anonymous.</remarks>
        public string Authorization { get; set; }

        public int ImageMaxWidthPx { get; set; }

        public int ImageMaxHeightPx { get; set; }

        public int ImageSkeletonMaxWidthPx { get; set; }

        public int ImageSkeletonMaxHeightPx { get; set; }

        public int ImageThumbnailMaxWidthPx { get; set; }

        public int ImageThumbnailMaxHeightPx { get; set; }

        public string ResourceFolderPath { get; set; }

        public string DefaultFolderName { get; set; }
    }
}