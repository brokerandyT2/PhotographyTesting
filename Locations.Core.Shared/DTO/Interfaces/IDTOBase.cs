using CommunityToolkit.Mvvm.ComponentModel;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.DTO.Interfaces
{
    public interface IDTOBase
    {
        event EventHandler<AlertEventArgs> RaiseAlert;
    }
   
}
