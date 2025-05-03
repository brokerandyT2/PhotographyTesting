using CommunityToolkit.Mvvm.ComponentModel;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.DTO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.DTO
{
    public partial class DTOBase : ObservableObject, IDTOBase
    {
        public event EventHandler<AlertEventArgs> RaiseAlert;

        public DTOBase()
        { 
            this.RaiseAlert += OnAlertRaised;
        }

        private void OnAlertRaised(object? sender, AlertEventArgs e)
        {
            RaiseAlert?.Invoke(this, e);
        }

        protected virtual void OnAlertRaised(string title, string message)
        {
            RaiseAlert?.Invoke(this, new AlertEventArgs(title, message));
        }
    }
}
