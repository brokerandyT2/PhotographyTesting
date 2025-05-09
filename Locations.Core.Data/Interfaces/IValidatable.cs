using System.Collections.Generic;

namespace Locations.Core.Data.Interfaces
{
    /// <summary>
    /// Interface for entities that can validate themselves
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Validates the entity
        /// </summary>
        /// <param name="errors">List of validation errors, if any</param>
        /// <returns>True if validation passed, false otherwise</returns>
        bool Validate(out List<string> errors);
    }
}