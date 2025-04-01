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

            query.SaveItem(model);
            return model;
        }
        public TipViewModel Save(TipViewModel model, bool returnNew)
        {

            var x = Save(model);
            return returnNew ? new TipViewModel() : x;
        }
        public TipViewModel Get(int id)
        {

            return query.GetItem<TipViewModel>(id);
        }

        public bool Delete(TipViewModel model)
        {

            var x = query.DeleteItem(model);
            return x != 420 ? true : false;
        }
        public bool Delete(int id)
        {
            var model = query.GetItem<TipViewModel>(id);
            var x = query.DeleteItem(model);
            return x != 420 ? true : false;
        }
        public bool Delete(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        public TipDisplayViewModel GetDisplay()
        {
            TipDisplayViewModel model = new TipDisplayViewModel();
            model.Displays = GetAllTips();

            return model;
        }

        private List<TipTypeViewModel> GetAllTips()
        {
            return (List<TipTypeViewModel>)query.dataB.Table<TipTypeViewModel>().Where(x => x.Name != null).ToListAsync().Result;
        }

        public TipDisplayViewModel PopulateTips(int id)
        {
            TipDisplayViewModel toReturn = new TipDisplayViewModel();
            toReturn.Displays = GetAllTips();
            var x = Get(id);
            toReturn.ISO = x.ISO;
            toReturn.Fstop = x.Fstop;
            toReturn.Shutterspeed = x.Shutterspeed;
            toReturn.Title  = x.Title;
            toReturn.Content = x.Content;
            toReturn.I8n = x.I8n;
            toReturn.TipTypeID = x.TipTypeID;
            toReturn.ID = x.ID;
            return toReturn;
           

        }
    }
}
