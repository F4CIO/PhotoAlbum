using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Common;
using System.IO;
using Common.Entities;

namespace UIWeb
{
    public partial class PhotoDetails : System.Web.UI.Page
    {
        public static string GetCurrentPhotoFilePath()
        {
            string returnValue = null;
        
                returnValue = HttpContext.Current.Request.QueryString[Constants.QueryStringKey_CurrentPhoto].ToString();
                returnValue = HttpUtility.UrlDecode(returnValue);
          
            return returnValue;
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
           
                if (!this.IsPostBack)
                {
                    string currentPhotoFilePath = GetCurrentPhotoFilePath();

                    this.aGoBack.InnerText = "[ Go back ]";//TODO:implement localization
                    this.aGoBack.HRef = string.Format("{0}?{1}={2}",
                        Constants.PageFileName_PhotoFolder,
                        Constants.QueryStringKey_CurrentAlbum,
                        Helper.GetParentFolderPath(currentPhotoFilePath)
                        );

                    this.lblCaption.Text = Path.GetFileName(currentPhotoFilePath);

                    this.imgPhoto.ImageUrl = string.Format("~/{0}?{1}={2}",
                        Constants.PageFileName_LocalFileHandler,
                        Constants.QueryStringKey_CurrentPhoto,
                        HttpUtility.UrlEncode(currentPhotoFilePath)
                        );


                    Photo photo = BusinessLogic.PhotosHandler.Get(currentPhotoFilePath, false);
                    this.lblResolution.Text = string.Format("Max Resolution: {0} x {1} pixels",//TODO:implement localization
                        photo.Width,
                        photo.Height
                        );
                    this.aDownloadHiRes.InnerText = "(download in max resolution)";//TODO:implement localization
                    this.aDownloadHiRes.HRef = string.Format("~/{0}?{1}={2}&{3}={4}",
                        Constants.PageFileName_LocalFileHandler,
                        Constants.QueryStringKey_CurrentPhoto,
                        HttpUtility.UrlEncode(currentPhotoFilePath),
                        Constants.QueryStringKey_FullSize,
                        true.ToString()
                        );
					this.aDownloadHiRes2.HRef = this.aDownloadHiRes.HRef;
                    this.lblSize.Text = string.Format("Size: {0:F2} kB", //TODO:implement localization
                        Convert.ToDecimal(photo.Size) / 1024
                        );


                    Photo previousPhoto = BusinessLogic.PhotosHandler.GetPreviousPhoto(currentPhotoFilePath, false);
                    this.aPrevious.Visible = previousPhoto != null;
                    if(previousPhoto!=null)
                    {
                    this.aPrevious.InnerText = "< Previous";
                    this.aPrevious.HRef = string.Format("{0}?{1}={2}",
                        Constants.PageFileName_PhotoDetails,
                        Constants.QueryStringKey_CurrentPhoto,
                        HttpUtility.UrlEncode( previousPhoto.filePath)
                        );
                    }

                    Photo nextPhoto = BusinessLogic.PhotosHandler.GetNextPhoto(currentPhotoFilePath, false);
                    this.aNext.Visible = nextPhoto != null;
                    if (nextPhoto != null)
                    {
                        this.aNext.InnerText = "Next >";
                        this.aNext.HRef = string.Format("{0}?{1}={2}",
                            Constants.PageFileName_PhotoDetails,
                            Constants.QueryStringKey_CurrentPhoto,
                            HttpUtility.UrlEncode(nextPhoto.filePath)
                            );
                    }
                }
           
            
        }
    }
}
