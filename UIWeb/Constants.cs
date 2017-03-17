using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Common;

namespace UIWeb
{
    public class Constants
    {
        public const string QueryStringKey_CurrentAlbum = "Album";
        public const string QueryStringKey_CurrentPhoto = "Photo";
        public const string QueryStringKey_OriginalPhotoFilePath = "OriginalPhotoFilePath";
        public const string QueryStringKey_IsFolder = "IsFolder";
        public const string QueryStringKey_FullSize = "FullSize";

        public const string PageFileName_PhotoFolder = "PhotoFolder.aspx";
        public const string PageFileName_PhotoDetails = "PhotoDetails.aspx";
        public const string PageFileName_LocalFileHandler = "LocalFileHandler.ashx";
    }
}
