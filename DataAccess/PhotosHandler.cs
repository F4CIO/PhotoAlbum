using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Entities;
using System.IO;
using System.Drawing;

namespace DataAccess
{
    public class PhotosHandler
    {
        public static List<Photo> GetAllPhotos(string baseFolderPath)
        {
            List<Photo> photos = new List<Photo>();
                      

                List<string> allFilePaths = Helper.GetFilePaths(baseFolderPath);
                foreach (string filePath in allFilePaths)
                {
                    if (FileTypes.IsSupported(filePath))
                    {
                        Photo photo = new Photo();
                        photo.filePath = filePath;
                        photo.thumbnail = null;
                        photos.Add(photo);
                    }
                }
         
            return photos;
        }

        public static List<FolderItem> GetAll(string baseFolderPath)
        {
            List<FolderItem> folderItems = new List<FolderItem>();
          
                
                List<string> folderPaths =  Helper.GetFolderPaths(baseFolderPath);
                foreach(string folderPath in folderPaths)
                {
                    Folder folder = new Folder();
                    folder.filePath = folderPath;
                    folderItems.Add(folder);
                }

                List<string> filePaths = Helper.GetFilePaths(baseFolderPath);
                foreach(string filePath in filePaths)
                {
                    if (FileTypes.IsSupported(filePath))
                    {
                        Photo photo = new Photo();
                        photo.filePath = filePath;
                        photo.thumbnail = null;
                        folderItems.Add(photo);
                    }
                }
          
            return folderItems;
        }

        public static bool ThumbnailCallback()
        {
            return false;
        }

        public static void GetThumbnail(Photo photo)
        {
           
                Image image = Image.FromFile(photo.filePath);
                Settings settings = SettingsHandler.Get();
                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                photo.thumbnail = image.GetThumbnailImage(settings.thumbnailWidth, settings.thumbnailHeight, myCallback, IntPtr.Zero);
          
        }

        public static Image GetImage(string filePath)
        {
            Image  image = null;
			//try
			//{
                image = Image.FromFile(filePath);
			//}
			//catch (Exception exception)
			//{
			//	ExceptionHandling.Handle(exception, ExceptionHandlingPolicies.DAL_Wrap_Policy);
			//}
            return image;
        }

        public static Photo Get(string filePath, bool loadImageAlso)
        {                     
             Photo photo = null;
           
                 photo = new Photo();
                 FileInfo fileInfo = new FileInfo(filePath);
                 photo.filePath = filePath;
                 photo.Size = fileInfo.Length;
				 if (photo.Size > 0)
				 {
					 Image image = Image.FromFile(filePath);
					 photo.Width = image.Width;
					 photo.Height = image.Height;
					 if (loadImageAlso)
					 {
						 photo.image = image;
					 }
					 else
					 {
						 image.Dispose();
					 }
				 }


	        return photo; 
         
        }
    }
}
