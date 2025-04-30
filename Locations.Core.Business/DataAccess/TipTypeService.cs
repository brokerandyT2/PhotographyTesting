using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Queries;
using Locations.Core.Shared.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess
{
    public class TipTypeService : ITipTypeService
    {
        TipTypesQuery<TipTypeViewModel> query = new TipTypesQuery<TipTypeViewModel>();
        public TipTypeService()
        {
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }
        public bool Delete(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }
    }
}
