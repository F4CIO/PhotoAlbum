using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;

namespace BusinessLogic
{
    public class SettingsHandler
    {
        public static Common.Entities.Settings Get()
        {
            Common.Entities.Settings settings = null;
			//try
			//{
                settings = DataAccess.SettingsHandler.Get();
			//}
			//catch (Exception exception)
			//{
			//	ExceptionHandling.Handle(exception, ExceptionHandlingPolicies.BL_Log_And_Replace_Policy);
			//}
            return settings;
        }
    }
}
