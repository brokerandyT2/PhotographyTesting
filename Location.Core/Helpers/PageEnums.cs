﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Core.Helpers
{
    [Obsolete("This enum really isn't obsolete.  This is just here so you can remember that there is a Base.ContentPageBase in every UI project.")]
    public enum PageEnums
    {
        WeatherDisplay,
        ExposureCalculator,
        LightMeter,
        SunLocation,
        SceneEvaluation,
        SunCalculations,
        AddLocation,
        ListLocations,
        Settings,
        Tips
    }
}
