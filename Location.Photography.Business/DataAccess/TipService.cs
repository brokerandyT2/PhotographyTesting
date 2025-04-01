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
namespace Location.Photography.Business.DataAccess
{
    public class TipService : ITipService<TipViewModel>
    {
        TipQuery<TipViewModel> query = new TipQuery<TipViewModel>();   
        public bool Delete(TipViewModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        public TipViewModel Get(int id)
        {
            return query.GetItem<TipViewModel>(id);
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
