using Location.Photography.Business.DataAccess.Interfaces;
using Location.Photography.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Photography.Shared.ViewModels;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Data.Queries;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Microsoft.Extensions.Logging;
using static Locations.Core.Shared.Customizations.Alerts.Implementation.AlertService;
using Locations.Core.Business.DataAccess;
namespace Location.Photography.Business.DataAccess
{
    public class TipService : ServiceBase<TipViewModel>, ITipService<TipViewModel>
    {


        // Now you can construct LoggerService


        private IAlertService alertServ;
        private ILoggerService loggerService;
        TipQuery<TipViewModel> query = new TipQuery<TipViewModel>(new AlertService(), new LoggerService(new ServiceCollection().AddLogging().BuildServiceProvider().GetRequiredService<ILogger<LoggerService>>()));   
        public bool Delete(TipViewModel model)
        {
            throw new NotImplementedException();
        }
        public TipService(IAlertService alertServ, ILoggerService loggerService) : this()
        {
            this.alertServ = alertServ;
            this.loggerService = loggerService;
        }

        public TipService() {
            alertServ.AlertRaised += AlertServ_AlertRaised;
        }

        private void AlertServ_AlertRaised(object? sender, AlertEventArgs e)
        {
            RaiseError(new Exception(e.Message));
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }
        public event EventHandler<AlertEventArgs> AlertRaised;
        public TipViewModel Get(int id)
        {
            try
            {
                return query.GetItem<TipViewModel>(id);
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return new TipViewModel();
            }
        }

        public TipViewModel Save(TipViewModel model)
        {
            throw new NotImplementedException();
        }

        public TipViewModel Save(TipViewModel model, bool returnNew)
        {
            throw new NotImplementedException();
        }
    }
}
