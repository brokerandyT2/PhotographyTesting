using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Core.Platforms.Android.Interface
{
    public interface IAmbientLightSensorService
    {
        public event EventHandler<float> LightLevelChanged;
        void StartListening();
        void StopListening();
    }
}
