using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Common;
using Common.Entities;
using BusinessLogic;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace UIWeb
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LocalFileHandler : IHttpHandler
    {
        public static string GetCurrentPhotoFilePath()
        {
            string returnValue = null;
          
                returnValue = HttpContext.Current.Request.QueryString[Constants.QueryStringKey_CurrentPhoto].ToString();
                returnValue = HttpUtility.UrlDecode(returnValue);
         
            return returnValue;
        }

        public static bool InFullSize()
        {
            bool returnValue = false;
           
                returnValue = Convert.ToBoolean( HttpContext.Current.Request.QueryString[Constants.QueryStringKey_FullSize] );                
          
            return returnValue;
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                Image image = null;
                if (InFullSize())
                {
                    Photo photo = BusinessLogic.PhotosHandler.Get(GetCurrentPhotoFilePath(), true);
                    image = photo.image;
                }
                else
                {
                    image = BusinessLogic.PhotosHandler.GetPreviewImage(GetCurrentPhotoFilePath());
                }                
                
                HttpContext.Current.Response.ContentType = "image/jpeg";
                image.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Jpeg);
                //Helper.SendFileToResponseStream(GetCurrentPhotoFilePath());
            }
            catch (Exception exception)
            {
                try
                {
                    Settings settings = SettingsHandler.Get();
                    string errorImageFilePath = HttpContext.Current.Server.MapPath(settings.errorImageFilePath);
                    Helper.SendFileToResponseStream(errorImageFilePath);
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
