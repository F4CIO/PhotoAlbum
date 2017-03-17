using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common.Entities
{

    public class FileTypes
    {
        public static bool IsSupported(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToUpper().Replace(".",string.Empty);

            string[] supportedFileTypes = Enum.GetNames(typeof(FileTypesSupported));

            bool found = false;
            System.Collections.IEnumerator myEnumerator = supportedFileTypes.GetEnumerator();
            while ((myEnumerator.MoveNext()) && (myEnumerator.Current != null && !found))
            {
                if (myEnumerator.Current.ToString().ToUpper() == extension)
                {
                    found = true;
                }
            }
            return found;
        }
    }

    public enum FileTypesSupported
    {
            Jpeg,
            Jpg,    
            Png,
            Bmp,
            Gif,
            Emf,
            Exif,
            Tiff,
            Tif,
            Wmf
    }

}
