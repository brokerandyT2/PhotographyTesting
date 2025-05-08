using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Data.Interfaces
{
    /// <summary>
    /// Interface for entities that can validate themselves
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Validates the entity and returns whether it's valid
        /// </summary>
        /// <param name="errors">List of validation errors if invalid</param>
        /// <returns>True if valid, false if invalid</returns>
        bool Validate(out List<string> errors);
    }
}