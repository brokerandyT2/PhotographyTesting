using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Queries;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;
using Microsoft.Extensions.Logging;

namespace Locations.Core.Business.DataAccess
{
    public class TipTypeService : ServiceBase<TipTypeViewModel>, ITipTypeService
    {
        TipTypesQuery<TipTypeViewModel> query = new TipTypesQuery<TipTypeViewModel>(new AlertService(), new LoggerService(new ServiceCollection().AddLogging().BuildServiceProvider().GetRequiredService<ILogger<LoggerService>>()));
        public event EventHandler<AlertEventArgs> AlertRaised;
        private IAlertService alertServ;
        private ILoggerService loggerService;
        public TipTypeService()
        {
        }
        public TipTypeService(IAlertService alertServ, ILoggerService log): this()
        {
            this.alertServ = alertServ;
            this.loggerService = log;
        }
        public TipTypeViewModel Save(TipTypeViewModel model)
        {
            try
            {
                query.SaveItem(model);
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
                return query.GetItem<TipTypeViewModel>(id);
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
                return (List<TipTypeViewModel>)query.GetItems<TipTypeViewModel>();
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
