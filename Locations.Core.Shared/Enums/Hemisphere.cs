using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.Enums
{
    public static class Hemisphere
    {
        public enum HemisphereChoices
        {
            North,
            South
        }
        public static string Name(this HemisphereChoices theEnum)
        {
            return Enum.GetName(typeof(HemisphereChoices), theEnum).ToLower();
        }
        public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => input[0].ToString().ToUpper() + input.Substring(1)
        };



    }
}
