using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Plugin.MauiMTAdmob.Interfaces;
namespace Locations.Core.Business.Advertising
{
    public class Advertising
    {
        private IAlertService alertServ;
        private ILoggerService loggerService;
        public Advertising(IAlertService alertServ, ILoggerService loggerService)
        {
            this.alertServ = alertServ;
            this.loggerService = loggerService;
        }

        public Advertising() { }
        public void GetAd() { }
        public void GetAds() { }

        public void SetConcent()
            { }
    }
}
