using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModelServices
{
    public interface IValidatable
    {
        bool Validate(out List<string> errors);
    }
}
