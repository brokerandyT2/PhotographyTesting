using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.Enums
{
    public static class SubscriptionType
    {
        public enum SubscriptionTypeEnum
        {
            Free = 0,
            Premium = 1,
            Professional = 2
        }
        public static string Name(this SubscriptionTypeEnum theEnum)
        {
            return Enum.GetName(typeof(SubscriptionTypeEnum), theEnum).ToLower();
        }
    }
}
