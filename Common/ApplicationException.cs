using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace Common
{
    class ApplicationException:Exception
    {
        public ApplicationException(string message):base(message)             
        {
          //
        }
    }

 
}
