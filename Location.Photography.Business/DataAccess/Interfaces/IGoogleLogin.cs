using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Business.DataAccess.Interfaces
{
    interface IGoogleLogin<T> : IBaseService<T> where T : class, new()
    {
    }
}
