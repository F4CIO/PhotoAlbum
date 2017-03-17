using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;

using Common.Entities;
using Common;
using System.Diagnostics;

namespace DataAccess
{
    public class SettingsHandler
    {
        private static Settings settings = null;

      

        /// <summary>
        /// Gets settings from cache or if it not exists loads it from data source.
        /// </summary>
        /// <returns></returns>
        public static Settings Get()
        {
            if (settings == null)
            {
                settings = Load();
            }
            return settings;
        }

        /// <summary>
        /// Forces loading of settings from data source and returns it. If settings are not found defaults are created and returned.
        /// </summary>
        /// <returns></returns>
        public static Common.Entities.Settings Load()
        {
            Common.Entities.Settings settings = null;
       
			

				settings = new Settings();

				settings.photosFolder = ConfigurationManager.AppSettings["photosFolder"];
				settings.errorImageFilePath = ConfigurationManager.AppSettings["errorImageFilePath"];
				settings.folderImageFilePath = ConfigurationManager.AppSettings["folderImageFilePath"];
				settings.thumbnailCountPerPage = int.Parse(ConfigurationManager.AppSettings["thumbnailCountPerPage"]);
				settings.thumbnailCountInRow = int.Parse(ConfigurationManager.AppSettings["thumbnailCountInRow"]);
				settings.thumbnailWidth = int.Parse(ConfigurationManager.AppSettings["thumbnailWidth"]);
				settings.thumbnailHeight = int.Parse(ConfigurationManager.AppSettings["thumbnailHeight"]);
				settings.previewWidth = int.Parse(ConfigurationManager.AppSettings["previewWidth"]);
				settings.previewHeight = int.Parse(ConfigurationManager.AppSettings["previewHeight"]);
					           
          
            return settings;
        }
    }
}
