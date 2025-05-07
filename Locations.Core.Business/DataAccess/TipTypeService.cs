using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Queries;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.ViewModels;
using Microsoft.Extensions.Logging;

namespace Locations.Core.Business.DataAccess
{
    public class TipTypeService : ServiceBase<TipTypeViewModel>, ITipTypeService
    {
        TipTypesQuery<TipTypeViewModel> query;
        public event EventHandler<AlertEventArgs> AlertRaised;
        private IAlertService alertServ;
        public TipTypeService()
        {
            query = new TipTypesQuery<TipTypeViewModel>();
        }
        public TipTypeService(IAlertService alertServ) : this()
        {
            this.alertServ = alertServ;

        }

        public TipTypeViewModel Save(TipTypeViewModel model)
        {
            try
            {
                query.SaveItem(model);
                if (model.IsError)
                {
                    RaiseError(new Exception(model.alertEventArgs.Message));
                }
                return model;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return model;

            }
        }
        public TipTypeViewModel Save(TipTypeViewModel model, bool returnNew)
        {
            try
            {
                var x = Save(model);
                if (x.IsError || model.IsError)
                {
                    RaiseError(new Exception(x.alertEventArgs.Message));
                }
                return returnNew ? new TipTypeViewModel() : x;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return model;
            }
        }
        public TipTypeViewModel Get(int id)
        {
            try
            {

                var x =  query.GetItem<TipTypeViewModel>(id);
                if (x.IsError)
                {
                    RaiseError(new Exception(x.alertEventArgs.Message));
                }
                return x;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return new TipTypeViewModel();
            }
        }
        public List<TipTypeViewModel> GetAllTips()
        {
            try
            {
                var x = (List<TipTypeViewModel>)query.GetItems<TipTypeViewModel>();
                foreach (var z in x)
                {
                    if (z.IsError)
                    {
                        RaiseError(new Exception(z.alertEventArgs.Message));
                    }
                }
                return x;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return new List<TipTypeViewModel>();
            }

        }

        public bool Delete(TipTypeViewModel model)
        {
            try
            {
                var x = query.DeleteItem(model);
                if (x == 420)
                {
                    RaiseError(new Exception(query.alertEventArgs.Message));
                    return false;
                }
                return x != 420 ? true : false;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return false;
            }

        }
        public bool Delete(int id)
        {
            try
            {
                var model = query.GetItem<TipTypeViewModel>(id);
                var x = query.DeleteItem(model);
                if (x == 420)
                {
                    RaiseError(new Exception("Error Deleting Tip Type"));
                    return false;
                }
                return x != 420 ? true : false;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return false;
            }
        }
        public bool Delete(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }
    }
}
