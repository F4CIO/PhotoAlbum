using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{
    public class Settings
    {
        public string photosFolder;

        public string errorImageFilePath;
        public string folderImageFilePath;

        public int thumbnailCountPerPage;
        public int thumbnailCountInRow;
        public int thumbnailWidth;
        public int thumbnailHeight;

        public int previewWidth;
        public int previewHeight;

        public Settings()
        {
            this.photosFolder = @"D:\Memoirs";

            this.errorImageFilePath = @"~/Images/Error.jpg";
            this.folderImageFilePath = @"~/Images/Folder.jpg";

            this.thumbnailCountPerPage = 12;
            this.thumbnailCountInRow = 3;
            this.thumbnailWidth = 150;
            this.thumbnailHeight = 150;

            this.previewWidth = 640;
            this.previewHeight = 480;

        }
    }
}
