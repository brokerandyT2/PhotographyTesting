using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess.Events
{
    public delegate void ServiceEventHandler(object sender, ServiceEventArgs e);
}
