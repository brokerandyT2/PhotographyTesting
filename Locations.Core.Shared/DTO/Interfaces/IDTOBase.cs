using Locations.Core.Shared.Alerts.Implementation;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Locations.Core.Shared.DTO.Interfaces
{
    public interface IDTOBase
    {
        event EventHandler<AlertEventArgs> RaiseAlert;
        bool IsError { get; set; }
        AlertEventArgs alertEventArgs { get; }
        ICommand RefreshCommand { get; }
        bool IsRefreshing { get; set; }
    }
}