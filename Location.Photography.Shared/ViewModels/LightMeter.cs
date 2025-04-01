using Location.Photography.Shared.ExposureCalculator;
using Location.Photography.Shared.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Shared.ViewModels
{
    public class LightMeter : ViewModelBase, ILightMeter
    {
        private double _exposureDivision;
        private string[] _fstops;
        private string[] _shutterspeeds;
        private string[] _isos;

        public double ExposureDivision
        {
            get { return _exposureDivision; }
            set
            {
                _exposureDivision = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExposureDivision)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShutterSpeed)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FStops)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ISOs)));
            }
        }
        public string[] ShutterSpeed

        {
            get
            {
                if(_exposureDivision == 1)
                {
                    return ShutterSpeeds.Full;
                }
                if (_exposureDivision == .5)
                {
                    return ShutterSpeeds.Halves;
                }
                else
                { return ShutterSpeeds.Thirds;
                }
            }
        }
        public string[] ISOs
        {
            get
            {
                if (_exposureDivision == 1)
                {
                    return Location.Photography.Shared.ExposureCalculator.ISOs.Full;
                }
                if (_exposureDivision == .5)
                {
                    return Shared.ExposureCalculator.ISOs.Halves;
                }
                else
                {
                    return Shared.ExposureCalculator.ISOs.Thirds;
                }
            }
        }
        public string[] FStops
        {
            get
            {
                if (_exposureDivision == 1)
                {
                    return Apetures.Full;
                }
                if (_exposureDivision == .5)
                {
                    return Apetures.Halves;
                }
                else
                {
                    return Apetures.Thirds;
                }
            }
        }
        public override event PropertyChangedEventHandler? PropertyChanged;
    }
}
