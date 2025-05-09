using System.Collections.Generic;

namespace Locations.Core.Shared.ViewModels
{
    /// <summary>
    /// Interface for models that support validation
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Validates the model and returns a result indicating if it's valid
        /// </summary>
        /// <param name="errors">List of validation errors if not valid</param>
        /// <returns>True if valid, false otherwise</returns>
        bool Validate(out List<string> errors);
    }
}