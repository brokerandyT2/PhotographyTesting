using CommunityToolkit.Mvvm.ComponentModel;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Location.Photography.Shared.ViewModels
{
    public class LightMeterViewModel : ObservableObject
    {
        public ObservableCollection<int> ISOs { get; set; }
        public ObservableCollection<double> Apertures { get; set; }

        public int SelectedISO { get; set; }
        public double SelectedAperture { get; set; }

        private double _needleRotation;
        public double NeedleRotation
        {
            get => _needleRotation;
            set => SetProperty(ref _needleRotation, value);
        }

        private double _peakNeedleRotation;
        public double PeakNeedleRotation
        {
            get => _peakNeedleRotation;
            set => SetProperty(ref _peakNeedleRotation, value);
        }

        private double _evValue;
        public double EVValue
        {
            get => _evValue;
            set => SetProperty(ref _evValue, value);
        }

        public LightMeterViewModel()
        {
            ISOs = new ObservableCollection<int>();
            var x = Photography.Shared.ExposureCalculator.ISOs.Full;
            foreach (var iso in x)
            {
                ISOs.Add(int.Parse(iso));
            }
            Apertures = new ObservableCollection<double> ();
            var z = Photography.Shared.ExposureCalculator.Apetures.Full;
            foreach (var iso in z)
            {
                Apertures.Add(double.Parse(iso.Replace("f","").Replace("/","")));
            }
            SelectedISO = 100;
            SelectedAperture = 2.8;

            // Start with EV = 0
            EVValue = 0;
            NeedleRotation = MapEVToNeedleRotation(EVValue);
            PeakNeedleRotation = NeedleRotation;
        }

        public void ProcessFrame(SKBitmap frame)
        {
            if (frame == null) return;

            double ev = CalculateExposureValue(frame);
            EVValue = ev;

            double angle = MapEVToNeedleRotation(ev);
            NeedleRotation = angle;

            if (angle > PeakNeedleRotation)
                PeakNeedleRotation = angle;
        }

        private double CalculateExposureValue(SKBitmap frame)
        {
            double brightness = 0;
            int pixels = 0;

            for (int y = 0; y < frame.Height; y += 10)
            {
                for (int x = 0; x < frame.Width; x += 10)
                {
                    var pixel = frame.GetPixel(x, y);
                    brightness += 0.2126 * pixel.Red + 0.7152 * pixel.Green + 0.0722 * pixel.Blue;
                    pixels++;
                }
            }

            brightness /= pixels;
            return brightness > 128 ? 0 : -5;
        }

        private double MapEVToNeedleRotation(double ev)
        {
            return 135 + (ev + 5) * (270 / 25); // Maps EV -5 to +20 onto 135–270 degrees
        }
    }
}
