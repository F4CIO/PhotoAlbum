using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Common.Entities
{
    public class Photo:FolderItem
    {
        public Image image;
        public Image thumbnail;
        public int Width;
        public int Height;
        public long Size;

    }
}
