using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.StorageSvc;
using Locations.Core.Shared.ViewModels.Interface;

namespace Locations.Core.Business.DataAccess
{
    public class ServiceBase<T> where T : IDTOBase, IBaseService<T>
    {
        public virtual void OnErrorOccurred(DataErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }
        public event DataErrorEventHandler? ErrorOccurred;




    }
}
