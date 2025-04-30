using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Queries;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess
{
    public class TipService : ITipService
    {
        TipQuery<TipViewModel> query = new TipQuery<TipViewModel>();
        public TipService()
        {
        }
        public TipViewModel Save(TipViewModel model)
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
        public TipViewModel Save(TipViewModel model, bool returnNew)
        {
            try
            {
                var x = Save(model);
                return returnNew ? new TipViewModel() : x;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new TipDisplayViewModel();
            }


        }
    }
}
