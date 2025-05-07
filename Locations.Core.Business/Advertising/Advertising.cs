using Locations.Core.Shared.Customizations.Alerts.Interfraces;
//using Plugin.MauiMTAdmob.Interfaces;
namespace Locations.Core.Business.Advertising
{
    public class Advertising
    {
        private IAlertService alertServ;

        public Advertising(IAlertService alertServ)
        {
            this.alertServ = alertServ;

        }

        public Advertising() { }
        public void GetAd() { }
        public void GetAds() { }

        public void SetConcent()
            { }
    }
}
