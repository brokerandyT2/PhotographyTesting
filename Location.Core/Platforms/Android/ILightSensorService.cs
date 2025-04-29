using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Core.Platforms.Android
{
    public interface ILightSensorService
    {
        void StartListening();
        void StopListening();
        float GetCurrentLux();
    }
}
