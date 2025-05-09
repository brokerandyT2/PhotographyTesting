//using Plugin.MauiMTAdmob.Interfaces;
using Location.Core.Helpers.AlertService;

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
