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
using System.IO;

namespace UIWeb
{
    public class Helper: Common.Helper
    {
        /// <summary>
        /// For '/subdir/page1.aspx' returns 'D:\Inetpub\wwwroot\MyWebApp\subdir\page1.aspx
        /// </summary>
        /// <param name="relativeFilePath"></param>
        /// <returns></returns>
        public static string GetAbsoluteLocalFilePath(string relativeFilePath)
        {
            string absoluteFilePath = null;
            relativeFilePath = relativeFilePath.Replace('/','\\');
            if (relativeFilePath.StartsWith(@"\"))
            {
                relativeFilePath = relativeFilePath.Remove(0, 1);
            }
            absoluteFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, relativeFilePath);
            return absoluteFilePath;
        }

        public static T FindControl<T>(Control startingControl, string id) where T : Control
        {
            // this is null by default
            T found = default(T);

            int controlCount = startingControl.Controls.Count;

            if (controlCount > 0)
            {
                for (int i = 0; i < controlCount; i++)
                {
                    Control activeControl = startingControl.Controls[i];
                    if (activeControl is T)
                    {
                        found = startingControl.Controls[i] as T;
                        if (string.Compare(id, found.ID, true) == 0) break;
                        else found = null;
                    }
                    else
                    {
                        found = FindControl<T>(activeControl, id);
                        if (found != null) break;
                    }
                }
            }
            return found;
        }

        /// <summary>
        /// Reads bypes from file to response stream and colse that response.
        /// </summary>
        /// <param name="localFilePath"></param>
        public static void SendFileToResponseStream(string localFilePath)
        {
            byte[] file = Helper.ReadBytesFromFile(localFilePath);

            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = "image/" + Path.GetExtension(localFilePath).Remove(0, 1);
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(Path.GetFileName(localFilePath)));
            HttpContext.Current.Response.BinaryWrite(file);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
        }
    }
}
