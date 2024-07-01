using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncfe.CodeTest
{
    public class AppSettings:IAppSettings
    {
        public bool IsFailoverModeEnabled
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["IsFailoverModeEnabled"]); }
        }
    }
}
