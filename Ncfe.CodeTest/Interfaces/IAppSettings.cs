using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncfe.CodeTest
{
    public interface IAppSettings
    {
        bool IsFailoverModeEnabled { get; }
    }
}
