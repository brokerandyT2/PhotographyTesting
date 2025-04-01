using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess.Interfaces
{
    public interface IBaseService<T> where T : class, new()
    {
        public T Save(T model);
        public T Save(T model, bool returnNew);
        public T Get(int id);

        public bool Delete(T model);
        public bool Delete(int id);
        public bool Delete(double latitude, double longitude);

    }
}
