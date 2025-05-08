using Location.Core.Helpers.AlertService;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.StorageSvc;
using Locations.Core.Shared.ViewModels.Interface;

namespace Locations.Core.Business.DataAccess
{
    public class ServiceBase<T> : IBaseService<T> where T: class, new()
    {
        public event DataErrorEventHandler? ErrorOccurred;

        event DataErrorEventHandler IBaseService<T>.ErrorOccurred
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public void OnErrorOccurred(DataErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }

        public T Save(T model)
        {
            throw new NotImplementedException();
        }

        public T Save(T model, bool returnNew)
        {
            throw new NotImplementedException();
        }

        public T Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        public void OnErrorOccurred(DataErrorEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
