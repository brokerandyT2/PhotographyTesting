using Location.Photography.Shared.ExposureCalculator;
using Location.Photography.Shared.ViewModels.Interfaces;
using static Location.Photography.Shared.ViewModels.ExposureCalculator;

namespace Location.Photography.Shared.ViewModels
    {
        public partial class ExposureCalculator : ViewModelBase, IExposureCalculator
        {
            private bool _showError;

            public bool ShowError
            {
                get => _showError;
                set
                {
                    _showError = value;
                    OnPropertyChanged(nameof(ShowError));
                }
            }

            // Map interface properties to the base class properties


            private string[] _fullStopApeature = Apetures.Full;
            private string[] _halfStopApeature = Apetures.Halves;
            private string[] _thirdStopApeature = Apetures.Thirds;

            private string[] _fullStopISO = ISOs.Full;
            private string[] _halfStopISO = ISOs.Halves;
            private string[] _thirdStopISO = ISOs.Thirds;

            private string[] _fullStopShutterSpeeds = ShutterSpeeds.Full;
            private string[] _halfStopShutterSpeeds = ShutterSpeeds.Halves;
            private string[] _thirdStopShutterSpeeds = ShutterSpeeds.Thirds;
            private FixedValue _fixedvalue;
            public enum FixedValue
            { ShutterSpeeds = 0, ISOs = 1, Apeature = 3 }
            public enum Divisions
            { Full, Half, Thirds }
            private Divisions _divisions;
            public Divisions FullHalfThirds
            {
                get { return _divisions; }
                set
                {
                    _divisions = value;
                    OnPropertyChanged(nameof(FullHalfThirds));
                    OnPropertyChanged(nameof(ApeaturesForPicker));
                    OnPropertyChanged(nameof(ISOsForPicker));
                    OnPropertyChanged(nameof(ShutterSpeedsForPicker));
                }
            }
            public string ISOResult
            {
                get => _isoResult;
                set
                {
                    _isoResult = value;
                    OnPropertyChanged(nameof(ISOResult));
                }
            }
            private string _isoResult;
            private string _shutterSpeedResult;
            private string _fstopResult;
            public string ShutterSpeedResult
            {
                get => _shutterSpeedResult;
                set
                {
                    _shutterSpeedResult = value;
                    OnPropertyChanged(nameof(ShutterSpeedResult));
                }
            }
            public string FStopResult { get => _fstopResult; set { _fstopResult = value; OnPropertyChanged(nameof(FStopResult)); } }
            public FixedValue ToCalculate
            {
                get { return _fixedvalue; }
                set
                {
                    _fixedvalue = value;
                    OnPropertyChanged(nameof(ToCalculate));
                }
            }
            public string[] ApeaturesForPicker
            {
                get
                {
                    if (FullHalfThirds == Divisions.Full)
                    {
                        return _fullStopApeature;
                    }
                    else if (FullHalfThirds == Divisions.Half)
                    {
                        return _halfStopApeature;
                    }
                    else
                    {
                        return _thirdStopApeature;
                    }
                }
            }
            public string[] ISOsForPicker
            {
                get
                {
                    if (FullHalfThirds == Divisions.Full)
                    {
                        return _fullStopISO;
                    }
                    else if (FullHalfThirds == Divisions.Half)
                    {
                        return _halfStopISO;
                    }
                    else
                    {
                        return _thirdStopApeature;
                    }
                }
            }

            public string[] ShutterSpeedsForPicker
            {
                get
                {
                    if (FullHalfThirds == Divisions.Full)
                    {
                        return _fullStopShutterSpeeds;
                    }
                    else if (FullHalfThirds == Divisions.Half)
                    {
                        return _halfStopShutterSpeeds;
                    }
                    else
                    {
                        return _thirdStopApeature;
                    }
                }
            }

            private string _fStopSelected;
            private string _iSOSelected;
            private string _shutterSpeedSelected;

            public string FStopSelected
            {
                get { return _fStopSelected; }
                set { _fStopSelected = value; OnPropertyChanged(nameof(FStopSelected)); }
            }
            public string ISOSelected
            {
                get { return _iSOSelected; }
                set
                {
                    _iSOSelected = value;
                    OnPropertyChanged(nameof(ISOSelected));
                }
            }

            public string ShutterSpeedSelected
            {
                get
                {
                    return _shutterSpeedSelected;
                }
                set
                {
                    _shutterSpeedSelected = value;
                    OnPropertyChanged(nameof(ShutterSpeedSelected));
                }
            }
            private string _oldshutterpseed;
            private string _oldfstop;
            private string _oldISO;
            public string OldShutterSpeed
            {
                get => _oldshutterpseed;
                set
                {
                    _oldshutterpseed = value;
                    OnPropertyChanged(nameof(OldShutterSpeed));
                }
            }
            public string OldFstop { get => _oldfstop; set { _oldfstop = value; OnPropertyChanged(nameof(OldFstop)); } }
            public string OldISO { get => _oldISO; set { _oldISO = value; OnPropertyChanged(nameof(OldISO)); } }

            private enum CalculateType
            {
                ISO,
                ShutterSpeed,
                Apeature
            }

            public void Calculate()
            {
                ExposureCalculator.ExposureTriangleValues etv = new ExposureCalculator.ExposureTriangleValues();
                etv.Aperture = OldFstop;
                etv.Shutter = OldShutterSpeed;
                etv.Iso = OldISO;

                ExposureIncrements steps;
                if (FullHalfThirds == Divisions.Full)
                {
                    steps = ExposureIncrements.Full;
                }
                else if (FullHalfThirds == Divisions.Half)
                {
                    steps = ExposureIncrements.Halves;
                }
                else
                {
                    steps = ExposureIncrements.Thirds;
                }
                Calculator c = new Calculator(steps);
                try
                {
                    // Clear any previous error
                    ErrorMessage = string.Empty;

                    switch (ToCalculate)
                    {
                        case FixedValue.Apeature:
                            ISOResult = ISOSelected;
                            ShutterSpeedResult = ShutterSpeedSelected;
                            FStopResult = c.CalculateNewApertureFromBaseExposure(etv, ShutterSpeedSelected, ISOSelected);
                            break;

                        case FixedValue.ShutterSpeeds:
                            ISOResult = ISOSelected;
                            FStopResult = FStopSelected;
                            ShutterSpeedResult = c.CalculateNewShutterFromBaseExposure(etv, FStopSelected, ISOSelected);
                            break;

                        case FixedValue.ISOs:
                            ISOResult = c.CalculateNewIsoFromBaseExposure(etv, FStopSelected, ShutterSpeedSelected);
                            FStopResult = FStopSelected;
                            ShutterSpeedResult = ShutterSpeedSelected;
                            break;
                        default:
                            break;
                    }

                    // Check if there was an error from the calculator
                    if (!string.IsNullOrEmpty(c.ErrorMessage))
                    {
                        ErrorMessage = c.ErrorMessage;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions during calculation
                    ErrorMessage = ex.Message;
                }
            }

            // The remaining code is unchanged...
            public enum ExposureIncrements
            {
                Thirds,
                Halves,
                Full
            }

            public class ExposureTriangleValues
            {
                public string Iso { get; set; }
                public string Aperture { get; set; }
                public string Shutter { get; set; }
            }

            public class Calculator
        {
            private readonly ExposureIncrements _increments;
            private readonly Dictionary<ExposureIncrements, IList<string>> _shutterSpeeds;
            private readonly Dictionary<ExposureIncrements, IList<string>> _isos;
            private readonly Dictionary<ExposureIncrements, IList<string>> _apertures;


            public Calculator(ExposureIncrements increments)
            {
                _increments = increments;
                _shutterSpeeds = new Dictionary<ExposureIncrements, IList<string>>();
                _isos = new Dictionary<ExposureIncrements, IList<string>>();
                _apertures = new Dictionary<ExposureIncrements, IList<string>>();
                var _isoFull = ISOs.Full.ToList();
                var _isoHalf = ISOs.Halves.ToList();
                var _isothirds = ISOs.Thirds.ToList();
                var _shutterSpeedsFull = ShutterSpeeds.Full.ToList();
                var _shutterSpeedsHalf = ShutterSpeeds.Halves.ToList();
                var _shutterSpeedsthirds = ShutterSpeeds.Thirds.ToList();
                _isos.Add(ExposureIncrements.Full, _isoFull);
                _isos.Add(ExposureIncrements.Halves, _isoHalf);
                _isos.Add(ExposureIncrements.Thirds, _isoHalf);
                _shutterSpeeds.Add(ExposureIncrements.Full, ShutterSpeeds.Full);
                _shutterSpeeds.Add(ExposureIncrements.Halves, ShutterSpeeds.Halves);
                _shutterSpeeds.Add(ExposureIncrements.Thirds, ShutterSpeeds.Thirds);
                _apertures.Add(ExposureIncrements.Full, Apetures.Full);
                _apertures.Add(ExposureIncrements.Halves, Apetures.Halves);
                _apertures.Add(ExposureIncrements.Thirds, Apetures.Thirds);
            }

            public ExposureIncrements GetIncrements() => _increments;

            public IList<string> GetShutterSpeeds() => _shutterSpeeds[_increments];

            public IList<string> GetApertures() => _apertures[_increments];

            public IList<string> GetIsos() => _isos[_increments];
            /// <summary>
            /// /// Calculates the new shutter speed based on the base exposure, final aperture, and final ISO
            /// </summary>
            /// <param name="baseExposure"></param>
            /// <param name="finalAperture"></param>
            /// <param name="finalIso"></param>
            /// <returns></returns>
            public string CalculateNewShutterFromBaseExposure(
                ExposureTriangleValues baseExposure,
                string finalAperture,
                string finalIso)
            {
                var values = GetShutterSpeeds();
                var baseIdx = values.IndexOf(baseExposure.Shutter);

                var offsetInStops = baseIdx +
                                  GetDifferenceInStops(GetApertures(), baseExposure.Aperture, finalAperture) +
                                  GetDifferenceInStops(GetIsos(), baseExposure.Iso, finalIso);

                if (offsetInStops < 0)
                {
                    return CalculateLongExposureShutterSpeed(offsetInStops, baseExposure).ToString();
                }

                return PluckVariableFromValues(values, baseIdx, offsetInStops);
            }
            /// <summary>
            /// /// Calculates the new aperture based on the base exposure, final shutter speed, and final ISO
            /// </summary>
            /// <param name="baseExposure"></param>
            /// <param name="finalShutter"></param>
            /// <param name="finalIso"></param>
            /// <returns></returns>
            public string CalculateNewApertureFromBaseExposure(
                ExposureTriangleValues baseExposure,
                string finalShutter,
                string finalIso)
            {
                var values = GetApertures();
                var baseIdx = values.IndexOf(baseExposure.Aperture);
                var offsetInStops = baseIdx +
                                   GetDifferenceInStops(GetShutterSpeeds(), baseExposure.Shutter, finalShutter) +
                                   GetDifferenceInStops(GetIsos(), baseExposure.Iso, finalIso);

                return PluckVariableFromValues(values, baseIdx, offsetInStops);
            }
            /// <summary>
            /// /// Calculates the new ISO based on the base exposure, final aperture, and final shutter speed
            /// </summary>
            /// <param name="baseExposure"></param>
            /// <param name="finalAperture"></param>
            /// <param name="finalShutter"></param>
            /// <returns></returns>
            public string CalculateNewIsoFromBaseExposure(
                ExposureTriangleValues baseExposure,
                string finalAperture,
                string finalShutter)
            {
                var values = GetIsos();
                var baseIdx = values.IndexOf(baseExposure.Iso);
                var offsetInStops = baseIdx +
                                   GetDifferenceInStops(GetShutterSpeeds(), baseExposure.Shutter, finalShutter) +
                                   GetDifferenceInStops(GetApertures(), baseExposure.Aperture, finalAperture);

                return PluckVariableFromValues(values, baseIdx, offsetInStops);
            }
            /// <summary>
            /// /// /// Plucks a variable from the values list based on the base index and the calculated index
            /// </summary>
            /// <param name="values"></param>
            /// <param name="baseIdx"></param>
            /// <param name="idx"></param>
            /// <returns></returns>
            /// <exception cref="OverexposedError"></exception>
            /// <exception cref="UnderexposedError"></exception>
            private string PluckVariableFromValues(IList<string> values, int baseIdx, int idx)
            {
                if (idx > values.Count - 1)
                {

                    ErrorMessage = new OverexposedError(idx, values, _increments).Message;
                }

                if (idx < 0)
                {

                    ErrorMessage = new UnderexposedError(idx, baseIdx, values, _increments).Message;
                }

                return values[idx];
            }
            public string ErrorMessage { get; set; } = string.Empty;
            /// <summary>
            /// /// /// Gets the difference in stops between the base value and the final value 
            /// </summary>
            /// <param name="values"></param>
            /// <param name="baseValue"></param>
            /// <param name="finalValue"></param>
            /// <returns></returns>
            public int GetDifferenceInStops(IList<string> values, string baseValue, string finalValue)
            {
                var baseIdx = values.IndexOf(baseValue);
                var actualIdx = values.IndexOf(finalValue);
                return baseIdx - actualIdx;
            }
            /// <summary>
            /// /// /// Calculates the long exposure shutter speed based on the offset in stops and the base exposure
            /// </summary>
            /// <param name="offsetInStops"></param>
            /// <param name="baseExposure"></param>
            /// <returns></returns>
            private double CalculateLongExposureShutterSpeed(int offsetInStops, ExposureTriangleValues baseExposure)
            {
                var stops = GetIncrementAsNumber(_increments);
                var factor = Math.Pow(2, stops);
                var baseShutterVal = baseExposure.Shutter;
                var baseShutterIdx = GetShutterSpeeds().IndexOf(baseShutterVal);

                var offset = Math.Abs(baseShutterIdx - offsetInStops);
                var seconds = Math.Round(Math.Pow(factor, offset) * ShutterStringToDecimal(baseShutterVal));

                return seconds;
            }
            /// <summary>
            /// /// /// Gets the increment as a number based on the increment type
            /// </summary>
            /// <param name="increment"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentException"></exception>
            private static double GetIncrementAsNumber(ExposureIncrements increment)
            {
                return increment switch
                {
                    ExposureIncrements.Full => 1,
                    ExposureIncrements.Thirds => 1.0 / 3.0,
                    ExposureIncrements.Halves => 1.0 / 2.0,
                    _ => throw new ArgumentException("Invalid increment value")
                };
            }
            /// <summary>
            ///     
            /// </summary>
            /// <param name="shutter"></param>
            /// <returns></returns>
            private static double ShutterStringToDecimal(string shutter)
            {
                if (shutter != null && shutter.EndsWith("\""))
                {
                    // Example: 2" -> 2
                    return double.Parse(shutter.TrimEnd('"'));
                }

                if (shutter != null && shutter.Contains("\""))
                {
                    // Example: 0"3 -> 1/3
                    return 1.0 / double.Parse(shutter[^1].ToString());
                }

                if (shutter != null && shutter.Contains("/"))
                {
                    // Example: 1/4 -> 0.25
                    var parts = shutter.Split('/');
                    return double.Parse(parts[0]) / double.Parse(parts[1]);
                }
                if (shutter == null) { shutter = "1"; }
                // Example: 3.2
                return double.Parse(shutter);
            }
            /// <summary>
            /// /// /// Formats the error message to full stops
            /// </summary>
            /// <param name="increments"></param>
            /// <param name="diff"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentException"></exception>
            private static double FormatErrorToFullStops(ExposureIncrements increments, int diff)
            {
                if (increments == ExposureIncrements.Full)
                {
                    throw new ArgumentException("formatErrorToFullStops received full increment type, but should only be passed ½ or ⅓");
                }

                var baseValue = increments == ExposureIncrements.Thirds ? 3 : 2;

                if (diff == baseValue)
                {
                    return 1;
                }

                if (diff % baseValue == 0)
                {
                    return diff / baseValue;
                }

                return Math.Round(1.0 / baseValue * diff, 1);
            }

            // Initialize the data structures with the same values as in the TypeScript code
            private static Dictionary<ExposureIncrements, IList<string>> InitializeShutterSpeeds()
            {
                return new Dictionary<ExposureIncrements, IList<string>>
                {
                    [ExposureIncrements.Thirds] = new List<string>
                {
                    "30\"", "25\"", "20\"", "15\"", "13\"", "10\"", "8\"", "6\"", "5\"", "4\"",
                    "3.2\"", "2.5\"", "2\"", "1.6\"", "1.3\"", "1\"", "0\"8", "0\"6", "0\"5", "0\"4",
                    "0\"3", "1/4", "1/5", "1/6", "1/8", "1/10", "1/13", "1/15", "1/20", "1/25",
                    "1/30", "1/40", "1/50", "1/60", "1/80", "1/100", "1/125", "1/160", "1/200",
                    "1/250", "1/320", "1/400", "1/500", "1/640", "1/800", "1/1000", "1/1250",
                    "1/1600", "1/2000", "1/2500", "1/3200", "1/4000", "1/5000", "1/6400", "1/8000"
                },
                    [ExposureIncrements.Halves] = new List<string>
                {
                    "30\"", "20\"", "15\"", "10\"", "8\"", "6\"", "4\"", "3\"", "2\"", "1.5\"",
                    "1\"", "0\"7", "0\"5", "0\"3", "1/4", "1/6", "1/8", "1/10", "1/15", "1/20",
                    "1/30", "1/45", "1/60", "1/90", "1/125", "1/180", "1/250", "1/350", "1/500",
                    "1/750", "1/1000", "1/1500", "1/2000", "1/3000", "1/4000", "1/6000", "1/8000"
                },
                    [ExposureIncrements.Full] = new List<string>
                {
                    "30\"", "15\"", "8\"", "4\"", "2\"", "1\"", "0\"5", "1/4", "1/8", "1/15",
                    "1/30", "1/60", "1/125", "1/250", "1/500", "1/1000", "1/2000", "1/4000", "1/8000"
                }
                };
            }

        }
    }
}
public class OverexposedError : Exception
{/// <summary>
 /// /// Constructor for OverexposedError
 /// </summary>
 /// <param name="requiredIdx"></param>
 /// <param name="values"></param>
 /// <param name="increments"></param>
    public OverexposedError(int requiredIdx, IList<string> values, ExposureIncrements increments)
        : base(FormatOverexposedMessage(requiredIdx, values, increments))
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="requiredIdx"></param>
    /// <param name="values"></param>
    /// <param name="increments"></param>
    /// <returns></returns>
    private static string FormatOverexposedMessage(int requiredIdx, IList<string> values, ExposureIncrements increments)
    {
        var diff = requiredIdx - (values.Count - 1);
        if (increments != ExposureIncrements.Full)
        {
            return $"The given parameters will result in an overexposed image. Using a value of {values[values.Count - 1]} " +
                   $"will still result in overexposure by {diff} {increments} stop increments " +
                   $"({FormatErrorToFullStops(increments, diff)} full stop(s))";
        }

        return $"The given parameters will result in an overexposed image. Using a value of {values[values.Count - 1]} " +
               $"will still result in overexposure by {diff} stop(s).";
    }
    /// <summary>
    /// /// Formats the error message to full stops
    /// </summary>
    /// <param name="increments"></param>
    /// <param name="diff"></param>
    /// <returns></returns>
    private static string FormatErrorToFullStops(ExposureIncrements increments, int diff)
    {
        if (increments == ExposureIncrements.Full)
        {
            return "formatErrorToFullStops received full increment type, but should only be passed ½ or ⅓";
        }
        return string.Empty;
    }
}
/// <summary>
/// /// Constructor for UnderexposedError
/// </summary>
public class UnderexposedError : Exception
{
    public UnderexposedError(int requiredIdx, int baseIdx, IList<string> values, ExposureIncrements increments)
        : base(FormatUnderexposedMessage(requiredIdx, baseIdx, values, increments))
    {
    }

    private static string FormatUnderexposedMessage(int requiredIdx, int baseIdx, IList<string> values, ExposureIncrements increments)
    {
        var diff = Math.Abs(baseIdx - requiredIdx);
        if (increments != ExposureIncrements.Full)
        {
            return $"The given parameters will result in an underexposed image. Using a value of {values[0]} " +
                   $"will still result in underexposure by {diff} {increments} stop increments " +
                   $"({FormatErrorToFullStops(increments, diff)} full stop(s))";
        }

        return $"The given parameters will result in an underexposed image. Using a value of {values[0]} " +
               $"will still result in underexposure by {diff} stop(s).";
    }
    private static double FormatErrorToFullStops(ExposureIncrements increments, int diff)
    {
        if (increments == ExposureIncrements.Full)
        {
            throw new ArgumentException("formatErrorToFullStops received full increment type, but should only be passed ½ or ⅓");
        }

        var baseValue = increments == ExposureIncrements.Thirds ? 3 : 2;

        if (diff == baseValue)
        {
            return 1;
        }

        if (diff % baseValue == 0)
        {
            return diff / baseValue;
        }

        return Math.Round(1.0 / baseValue * diff, 1);
    }
}