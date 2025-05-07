using Location.Photography.Business.DataAccess.Interfaces;
using Locations.Core.Business.DataAccess;
using Locations.Core.Data.Queries;
using Locations.Core.Shared;
using Locations.Core.Shared.Alerts.Implementation;
using Locations.Core.Shared.ViewModels;
namespace Location.Photography.Business.DataAccess
{
    public class TipService : ServiceBase<TipViewModel>, ITipService<TipViewModel>
    {


        // Now you can construct LoggerService



        TipQuery<TipViewModel> query = new TipQuery<TipViewModel>();   
        public bool Delete(TipViewModel model)
        {
            RaiseError(new NotImplementedException());
            return default;
        }
        public TipService() 
        {
            
       
        }
        public TipService(string email):this()
        {
            var q = new TipQuery<TipViewModel>();
            var x = q.GetItemByString<SettingViewModel>(MagicStrings.Email).Value;
            if (string.IsNullOrEmpty(x))
            {
                //loggerService.LogWarning($"Email is not set.  Cannot use encrypted database. Email Address {x}");
                throw new ArgumentException("Email is not set.  Cannot use encrypted database.");
            }
            query = new TipQuery<TipViewModel>();
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
                var x= query.GetItem<TipViewModel>(id);
            if(x.IsError)
                {
                    RaiseAlert(this, new Locations.Core.Shared.Customizations.Alerts.Implementation.AlertEventArgs(x.alertEventArgs.Title, x.alertEventArgs.Message, true));
                }
                return x;
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
