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
using Common.Entities;
using System.Collections.Generic;
using System.IO;
using BusinessLogic;

namespace UIWeb
{
    public class PhotosHandler
    {
        public static string GetCurrentAlbumPath()
        {
            string returnValue = null;
         
                returnValue = HttpContext.Current.Request.QueryString.Get(Constants.QueryStringKey_CurrentAlbum);
                returnValue = HttpUtility.UrlDecode(returnValue);
          
            return returnValue;
        }

        public static string GetCurrentAlbumName()
        {
            string albumName = string.Empty;
            try
            {
                Settings settings = SettingsHandler.Get();
                albumName = GetCurrentAlbumPath().Replace(settings.photosFolder, "").Remove(0, 1);
            }
            catch (Exception exception)
            {
                albumName = string.Empty;
            }
            return albumName;
        }

        public static DataSet GetAllInDataset()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            DataColumn dataColumn = new DataColumn("FilePath", typeof(string));
            dataTable.Columns.Add(dataColumn);
            DataColumn dataColumn2a = new DataColumn("FilePathEncoded", typeof(string));
            dataTable.Columns.Add(dataColumn2a);
            DataColumn dataColumn2 = new DataColumn("FileName", typeof(string));
            dataTable.Columns.Add(dataColumn2);
            DataColumn dataColumn3 = new DataColumn("IsFolder", typeof(bool));
            dataTable.Columns.Add(dataColumn3);
            DataColumn dataColumn4 = new DataColumn("Thumbnail", typeof(Image));
            dataTable.Columns.Add(dataColumn4);
            dataSet.Tables.Add(dataTable);

           
                string currentAlbumPath = GetCurrentAlbumPath();

                List<FolderItem> folderItems = BusinessLogic.PhotosHandler.GetAll(currentAlbumPath);
                foreach (FolderItem folderItem in folderItems)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["FilePath"] = folderItem.filePath;
                    dataRow["FilePathEncoded"] = HttpUtility.UrlEncode(folderItem.filePath);
                    dataRow["FileName"] = Path.GetFileName(folderItem.filePath);
                    if (folderItem is Photo)
                    {
                        dataRow["IsFolder"] = false;
                        dataRow["Thumbnail"] = (folderItem as Photo).thumbnail;
                    }
                    else if (folderItem is Folder)
                    {
                        dataRow["IsFolder"] = true;
                    }
                    dataTable.Rows.Add(dataRow);
                }
           
            return dataSet;
        }
    }
}
