using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

using Common.Entities;
using Common;
using BusinessLogic;
using System.Drawing;
using System.Drawing.Imaging;

namespace UIWeb
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ThumbnailHandler1 : IHttpHandler
    {
        public static string GetOriginalPhotoFilePath()
        {
            string originalPhotoFilePath = null;
            try
            {
                originalPhotoFilePath = HttpContext.Current.Request.QueryString.Get(Constants.QueryStringKey_OriginalPhotoFilePath);
                originalPhotoFilePath = HttpUtility.UrlDecode(originalPhotoFilePath);
            }
            catch (Exception exception)
            {
                try
                {
                    Settings settings = SettingsHandler.Get();
                    originalPhotoFilePath = settings.errorImageFilePath;
                }catch (Exception){}
                
            }
            return originalPhotoFilePath;
        }

        public static bool IsFolder()
        {
            bool isFolder = false;
          
                isFolder = Convert.ToBoolean( HttpContext.Current.Request.QueryString.Get(Constants.QueryStringKey_IsFolder) );
           
            return isFolder;
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string filePath = null;
                Settings settings = SettingsHandler.Get();
                if (IsFolder())
                {
                    
                    filePath = settings.folderImageFilePath;
                }
                else
                {
                    filePath = GetOriginalPhotoFilePath();
                }

                if (!filePath.StartsWith(settings.photosFolder))
                {
                    filePath = HttpContext.Current.Server.MapPath(filePath);
                }


                Image thumbnail = BusinessLogic.PhotosHandler.GetThumbnail(filePath);

                context.Response.ContentType = "image/jpeg";
                thumbnail.Save(context.Response.OutputStream, ImageFormat.Jpeg);
            }
            catch (Exception exception)
            {
                //At least try to show error thumbnail image
                try
                {
                    Settings settings = SettingsHandler.Get();
                    string errorImageFilePath = settings.errorImageFilePath;
                    if (!errorImageFilePath.StartsWith(settings.photosFolder))
                    {
                        errorImageFilePath = HttpContext.Current.Server.MapPath(errorImageFilePath);
                    }
                    Image thumbnail = BusinessLogic.PhotosHandler.GetThumbnail(errorImageFilePath);

                    context.Response.ContentType = "image/jpeg";
                    thumbnail.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                }
                catch (Exception) { }

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
