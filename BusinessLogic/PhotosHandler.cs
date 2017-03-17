using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

using Common;
using Common.Entities;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;



namespace BusinessLogic
{
    public class PhotosHandler
    {
        public static string GetLocalPhotoFolderPath(string relativePath)
        {
            string localPath = null;
            Settings settings = SettingsHandler.Get();
            if (string.IsNullOrEmpty(relativePath))
            {
                localPath = settings.photosFolder;
            }
            else
            {
                if (!relativePath.StartsWith(settings.photosFolder))
                {
                    localPath = Path.Combine(settings.photosFolder, relativePath);
                }
                else
                {
                    localPath = relativePath;
                }
            }
            return localPath;
        }

        public static List<FolderItem> GetAll(string relativeFolder)
        {
            List<FolderItem> returnValue = null;
          
                string folderPath = GetLocalPhotoFolderPath(relativeFolder);

                returnValue = DataAccess.PhotosHandler.GetAll(folderPath);
          
            return returnValue;
        }

        public static List<Photo> GetAllPhotos(string relativeFolder)
        {
            List<Photo> returnValue = null;
           
                string folderPath = GetLocalPhotoFolderPath(relativeFolder);

                returnValue = DataAccess.PhotosHandler.GetAllPhotos(folderPath);
           
            return returnValue;
        }



        public static bool ThumbnailCallback()
        {
            return false;
        }

         public static Image GetThumbnail(string filePath)
         {
             Image thumbnail = null;
           
                 Image image = DataAccess.PhotosHandler.GetImage(filePath);
                 
                 Settings settings = SettingsHandler.Get();
                   
                 Size newSize = Common.Helper.ResizeToFit(new Size(image.Width, image.Height), new Size(settings.thumbnailWidth, settings.thumbnailHeight), false);

                 Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                 thumbnail = image.GetThumbnailImage(newSize.Width, newSize.Height, myCallback, IntPtr.Zero);        

                 //From: http://209.85.173.132/search?q=cache:wIteYk0dkLEJ:daveweaver.net/notebook,permalink,CreateAScaledThumbnailImageInASPNET+asp.net+how+get+scalled+thumbnail&cd=1&hl=en&ct=clnk
                 //int thumbWidth = settings.thumbnailWidth;
                 //int srcWidth = image.Width;
                 //int srcHeight = image.Height;

                 //Decimal sizeRatio = ((Decimal)srcHeight / srcWidth);
                 //int thumbHeight = Decimal.ToInt32(sizeRatio * thumbWidth);
                 //Bitmap bmp = new Bitmap(thumbWidth, thumbHeight);
                 //System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
                 //gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                 //gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                 //gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                 //System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight);
                 //gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);                 
                 //thumbnail = bmp;
                 image.Dispose();
                                
          
             return thumbnail;
         }

         public static Image GetPreviewImage(string filePath)
         {
             Image previewImage = null;
			
                 Image image = DataAccess.PhotosHandler.GetImage(filePath);

                 Settings settings = SettingsHandler.Get();

                 Size newSize = Common.Helper.ResizeToFit(new Size(image.Width, image.Height), new Size(settings.previewWidth, settings.previewHeight), false);

                 //Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                 //previewImage = image.GetThumbnailImage(newSize.Width, newSize.Height, myCallback, IntPtr.Zero);
                 
                 Bitmap bmp = new Bitmap(newSize.Width, newSize.Height);
                 Graphics gr = Graphics.FromImage(bmp);
                 gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                 gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                 gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                 Rectangle rectDestination = new Rectangle(0, 0, newSize.Width, newSize.Height);
                 gr.DrawImage(image, rectDestination, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                 previewImage = bmp;
               
			 	image.Dispose();
			
             return previewImage;
         }



         public static Photo Get(string filePath, bool loadImageAlso)
         {
             Photo photo = null;
            
                 photo = DataAccess.PhotosHandler.Get(filePath, loadImageAlso);
           
             return photo; 
         }

         public static Photo GetPreviousPhoto(string currentPhotoFilePath, bool loadImageAlso)
         {
             Photo photo = null;
             bool photoFound = false;
            
                 string currentPhotoFolderPath = Helper.GetParentFolderPath(currentPhotoFilePath);
                 List<Photo> siblingPhotos = DataAccess.PhotosHandler.GetAllPhotos(currentPhotoFolderPath);
                 int i = 0;
                 while (i < siblingPhotos.Count && !photoFound)
                 {
                     if (siblingPhotos[i].filePath == currentPhotoFilePath)
                     {//current index found
                         photoFound = true;
                         if (i - 1 >= 0)
                         {//previous photo exist
                             photo = DataAccess.PhotosHandler.Get(siblingPhotos[i - 1].filePath, loadImageAlso);
                         }
                     }
                     i++;
                 }
           
             return photo; 
         }

         public static Photo GetNextPhoto(string currentPhotoFilePath, bool loadImageAlso)
         {
             Photo photo = null;
             bool photoFound = false;
            
                 string currentPhotoFolderPath = Helper.GetParentFolderPath(currentPhotoFilePath);
                 List<Photo> siblingPhotos = DataAccess.PhotosHandler.GetAllPhotos(currentPhotoFolderPath);
                 int i = 0;
                 while (i < siblingPhotos.Count && !photoFound)
                 {
                     if (siblingPhotos[i].filePath == currentPhotoFilePath)
                     {//current index found
                         photoFound = true;
                         if (i + 1 < siblingPhotos.Count)
                         {//previous photo exist
                             photo = DataAccess.PhotosHandler.Get(siblingPhotos[i + 1].filePath, loadImageAlso);
                         }
                     }
                     i++;
                 }
          
             return photo;
         }

    }
}
