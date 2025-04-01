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

            query.SaveItem(model);
            return model;
        }
        public TipTypeViewModel Save(TipTypeViewModel model, bool returnNew)
        {

            var x = Save(model);
            return returnNew ? new TipTypeViewModel() : x;
        }
        public TipTypeViewModel Get(int id)
        {

            return query.GetItem<TipTypeViewModel>(id);
        }
        public List<TipTypeViewModel> GetAllTips()
        { 
            return (List<TipTypeViewModel>)query.GetItems<TipTypeViewModel>();
        }

        public bool Delete(TipTypeViewModel model)
        {

            var x = query.DeleteItem(model);
            return x != 420 ? true : false;
        }
        public bool Delete(int id)
        {
            var model = query.GetItem<TipTypeViewModel>(id);
            var x = query.DeleteItem(model);
            return x != 420 ? true : false;
        }
        public bool Delete(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }
    }
}
