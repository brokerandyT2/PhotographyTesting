using Locations.Core.Shared.Alerts.Implementation;

namespace Locations.Core.Shared.DTO.Interfaces
{
    public interface IDTOBase
    {
        event EventHandler<AlertEventArgs> RaiseAlert;
    }
   
}
