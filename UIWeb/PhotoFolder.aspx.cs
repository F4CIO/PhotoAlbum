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
using System.Collections.Generic;

using Common.Entities;
using Common;
using BusinessLogic;
using System.Text;


namespace UIWeb
{
    public partial class PhotoFolder : System.Web.UI.Page
    {
        //public static List<FolderItem> itemsToShow;

        protected void Page_Init(object sender, EventArgs e)
        {
            //not reflected
            //DataPager datapager = this.pgrAlbum;//(DataPager)ListView1.FindControl("pgrAlbum");
            //if (datapager != null)
            //{
            //    Settings settings = SettingsHandler.Get();
            //    datapager.PageSize = settings.thumbnailCountPerPage;
            //}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string currentAlbumPath = PhotosHandler.GetCurrentAlbumPath();
				Settings settings = SettingsHandler.Get();
                HtmlGenericControl divGoBackOneLevel = Helper.FindControl<HtmlGenericControl>(this, "divGoBackOneLevel");
                if (divGoBackOneLevel != null)
                {
                    divGoBackOneLevel.Visible = false;
                    
                    if (!string.IsNullOrEmpty(currentAlbumPath) && currentAlbumPath != settings.photosFolder)
                    {
                        string parentFolderPath = Helper.GetParentFolderPath(PhotosHandler.GetCurrentAlbumPath());
                        divGoBackOneLevel.Visible = !string.IsNullOrEmpty(parentFolderPath);

                        if (divGoBackOneLevel.Visible)
                        {
                            HtmlAnchor aGoBackOneLevel = Helper.FindControl<HtmlAnchor>(this, "aGoBackOneLevel");
                            aGoBackOneLevel.InnerText = "[ Go back ]"; //TODO:implement localization
                            aGoBackOneLevel.HRef = GetThumbnailRedirectUri(true, parentFolderPath);

                            Label lblAlbumName = Helper.FindControl<Label>(this, "lblAlbumName");
                            lblAlbumName.Text = PhotosHandler.GetCurrentAlbumName();
                        }
                    }
                }
				ViewState["thumbnailCountInRow"] = settings.thumbnailCountInRow;
            }
        }

        public string GetThumbnailRedirectUri(bool isFolder, string filePath)
        {
            string currentUri = HttpContext.Current.Request.Url.AbsoluteUri;
            string redirectUri=currentUri;
           
                if (isFolder)
                {

                    //redirectUri = string.Format(@"{0}://{1}{2}?{3}={4}",
                    //    UriHelper.GetProtocol(currentUri),
                    //    UriHelper.GetAuthority(currentUri),
                    //    UriHelper.GetPath(currentUri),
                    //    Constants.QueryStringKey_CurrentAlbum,
                    //    HttpContext.Current.Server.UrlEncode(filePath)  );
                    // or:
                    redirectUri = string.Format("{0}?{1}={2}",
                        Request.Url.Query == string.Empty ? currentUri : currentUri.Replace(Request.Url.Query, ""),
                        Constants.QueryStringKey_CurrentAlbum,
                        HttpContext.Current.Server.UrlEncode(filePath));
                }
                else
                {
                    redirectUri = string.Format("~/{0}?{1}={2}",                        
                        Constants.PageFileName_PhotoDetails,
                        Constants.QueryStringKey_CurrentPhoto,
                        HttpContext.Current.Server.UrlEncode(filePath));
                }
           
            return redirectUri;
        }

        protected void ListView1_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Settings settings = SettingsHandler.Get();
				//ListView1.GroupItemCount = settings.thumbnailCountInRow; //see photoFolderItemsContainerMaxWidth

				if (ListView1.Items.Count == 0)
                {
                    Label lblNoPhotos = Helper.FindControl<Label>(ListView1, "lblNoPhotos");
                    if (lblNoPhotos != null)
                    {
                        lblNoPhotos.Text = "No photos here"; //TODO:implement localization
                    }
                }
            }
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRow currentRow = ((e.Item as ListViewDataItem).DataItem as DataRowView).Row;

            Panel pnlPhoto = (Panel)e.Item.FindControl("pnlPhoto");
            pnlPhoto.Width = SettingsHandler.Get().thumbnailWidth + 5;
            pnlPhoto.Height = SettingsHandler.Get().thumbnailHeight + 5;

            HtmlAnchor aPhotoImage = Helper.FindControl<HtmlAnchor>(e.Item, "aPhotoImage");//(HtmlGenericControl)pnlPhoto.FindControl("aPhotoImage");
            aPhotoImage.Attributes["href"] = GetThumbnailRedirectUri(Convert.ToBoolean(currentRow["IsFolder"]), currentRow["FilePathEncoded"].ToString());

            Image imgPhoto = Helper.FindControl<Image>(e.Item, "imgPhoto");
            imgPhoto.ImageUrl = string.Format("~/ThumbnailHandler.ashx?{0}={1}&{2}={3}",
                Constants.QueryStringKey_IsFolder,
                currentRow["IsFolder"], 
                Constants.QueryStringKey_OriginalPhotoFilePath,
                currentRow["FilePathEncoded"]);

            HtmlAnchor aPhotoCaption = Helper.FindControl<HtmlAnchor>(e.Item, "aPhotoCaption");
            aPhotoCaption.Attributes["href"] = aPhotoImage.Attributes["href"];
            aPhotoCaption.InnerText = currentRow["FileName"].ToString();
            aPhotoCaption.Title = currentRow["FileName"].ToString();
        }

        //--------------------------Here are some examples of eval use:
        //href ="<%#GetThumbnailRedirectUri(DataBinder.Eval(	,"FilePathEncoded").ToString()) %>"
        //ImageUrl='<%# "~/ThumbnailHandler.ashx?IsFolder="+Eval("IsFolder")+"&OriginalPhotoFilePath="+Eval("FilePathEncoded")%>'
        //href ="<%#GetThumbnailRedirectUri(DataBinder.Eval(Container.DataItem,"FilePathEncoded").ToString()) %>"
                                       
    }
}
