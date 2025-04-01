using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess.Interfaces
{
    public interface ILocationService<T> : IBaseService<T> where T : class, new()
    {
        public T GetLocation(double latitude, double longitude);

    }
}
