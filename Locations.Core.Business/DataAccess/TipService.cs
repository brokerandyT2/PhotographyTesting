using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Queries;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Animations;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess
{
    public class TipService : ServiceBase<TipViewModel>, ITipService
    {
        TipQuery<TipViewModel> query = new TipQuery<TipViewModel>(new AlertService(), new LoggerService(new ServiceCollection().AddLogging().BuildServiceProvider().GetRequiredService<ILogger<LoggerService>>()));
        private IAlertService alertServ;
        private ILoggerService loggerService;
        public TipService()
        {
        }
        public TipService(IAlertService alertServ, ILoggerService loggerService) : this()
        {
            this.alertServ = alertServ;
            this.loggerService = loggerService;
        }
        public TipService(IAlertService alertServ, ILoggerService loggerService, string email) : this(alertServ, loggerService)
        {
            var q = new TipQuery<TipViewModel>(alertServ, loggerService);
            var x = q.GetItemByString<SettingViewModel>(MagicStrings.Email).Value;
            if (string.IsNullOrEmpty(x))
            {
                loggerService.LogWarning($"Email is not set.  Cannot use encrypted database. Email Address {x}");
                throw new ArgumentException("Email is not set.  Cannot use encrypted database.");
            }
            query = new TipQuery<TipViewModel>(alertServ, loggerService, email);
        }
        public TipViewModel Save(TipViewModel model)
        {
            try
            {
                query.SaveItem(model);
                if (IsError)
                {
                    RaiseError(new Exception("Error Saving Tip Type"));
                                        return model;
                }
                return model;
            }
            catch (Exception ex)
            {
                IsError = true;
                RaiseError(ex);
                return model;
            }
        }
        public TipViewModel Save(TipViewModel model, bool returnNew)
        {
            try
            {
                var x = Save(model);
                return returnNew ? new TipViewModel() : x;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return model;
            }

        }
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

        public bool Delete(TipViewModel model)
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
                var model = query.GetItem<TipViewModel>(id);
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

        public TipDisplayViewModel GetDisplay()
        {
            try
            {
                TipDisplayViewModel model = new TipDisplayViewModel();
                model.Displays = GetAllTips();

                return model;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return new TipDisplayViewModel();
            }
        }

        private List<TipTypeViewModel> GetAllTips()
        {
            try
            {
                return (List<TipTypeViewModel>)query.dataB.Table<TipTypeViewModel>().Where(x => x.Name != null).ToListAsync().Result;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return new List<TipTypeViewModel>();
            }
        }

        public TipDisplayViewModel PopulateTips(int id)
        {
            try
            {
                TipDisplayViewModel toReturn = new TipDisplayViewModel();
                toReturn.Displays = GetAllTips();
                var x = Get(id);
                toReturn.ISO = x.ISO;
                toReturn.Fstop = x.Fstop;
                toReturn.Shutterspeed = x.Shutterspeed;
                toReturn.Title = x.Title;
                toReturn.Content = x.Content;
                toReturn.I8n = x.I8n;
                toReturn.TipTypeID = x.TipTypeID;
                toReturn.ID = x.ID;
                return toReturn;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return new TipDisplayViewModel();
            }


        }
    }
}
