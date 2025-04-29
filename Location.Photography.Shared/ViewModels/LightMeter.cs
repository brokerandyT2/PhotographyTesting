using Location.Photography.Shared.ViewModels.Interfaces;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using Timer = System.Timers.Timer;


namespace Location.Photography.Shared.ViewModels
{
    public class LightMeter : ViewModelBase, ILightMeter
    {

            private double _needleRotation;
            private double _targetRotation;
            private double _peakNeedleRotation;
            private double _evValue;
            private double _targetEV;
            private DateTime _lastPeakTime;
            private readonly Timer _smoothingTimer;

            public ObservableCollection<int> ISOs { get; } = new() { 100, 200, 400, 800, 1600 };
            public ObservableCollection<string> Apertures { get; } = new() { "1.8", "2.8", "4", "5.6", "8", "11", "16" };

            private int _selectedISO = 100;
            public int SelectedISO
            {
                get => _selectedISO;
                set
                {
                    if (_selectedISO != value)
                    {
                        _selectedISO = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string _selectedAperture = "2.8";
            public string SelectedAperture
            {
                get => _selectedAperture;
                set
                {
                    if (_selectedAperture != value)
                    {
                        _selectedAperture = value;
                        OnPropertyChanged();
                    }
                }
            }

            public double NeedleRotation
            {
                get => _needleRotation;
                set
                {
                    if (_needleRotation != value)
                    {
                        _needleRotation = value;
                        OnPropertyChanged();
                    }
                }
            }

            public double EVValue
            {
                get => _evValue;
                set
                {
                    if (_evValue != value)
                    {
                        _evValue = value;
                        OnPropertyChanged();
                    }
                }
            }

            public double PeakNeedleRotation { get; set; }

            public LightMeter()
            {
                _smoothingTimer = new Timer(16); // ~60fps
                _smoothingTimer.Elapsed += SmoothingTimer_Elapsed;
                _smoothingTimer.Start();
                _lastPeakTime = DateTime.UtcNow;
            }

            public void ProcessFrame(SKBitmap e)
            {
                try
                {
                    byte[] pixelBuffer = e.Bytes;



                    if (pixelBuffer == null || pixelBuffer.Length == 0)
                        return;

                    double brightness = CalculateBrightness(pixelBuffer);
                    double adjustedBrightness = AdjustBrightness(brightness, SelectedISO, SelectedAperture);
                    double newRotation = MapBrightnessToRotation(adjustedBrightness);

                    // Update Peak
                    if (newRotation > _peakNeedleRotation)
                    {
                        _peakNeedleRotation = newRotation;
                        _lastPeakTime = DateTime.UtcNow;
                    }

                    _targetRotation = newRotation;

                    // Calculate EV
                    _targetEV = CalculateEV(adjustedBrightness);
                }
                catch
                {
                    // Log or ignore
                }
            }

            private void SmoothingTimer_Elapsed(object sender, ElapsedEventArgs e)
            {
                const double smoothingFactor = 0.1;

                // Peak Hold Logic
                if ((DateTime.UtcNow - _lastPeakTime).TotalSeconds < 2)
                {
                    NeedleRotation += (_peakNeedleRotation - NeedleRotation) * smoothingFactor;
                }
                else
                {
                    NeedleRotation += (_targetRotation - NeedleRotation) * smoothingFactor;
                }

                // Smooth EV
                EVValue += (_targetEV - EVValue) * smoothingFactor;
            }

            private double CalculateBrightness(byte[] pixelBuffer)
            {
                double total = 0;
                for (int i = 0; i < pixelBuffer.Length; i++)
                {
                    total += pixelBuffer[i];
                }
                return total / pixelBuffer.Length; // 0-255
            }

            private double AdjustBrightness(double rawBrightness, int iso, string aperture)
            {
                double normalizedBrightness = rawBrightness / 255.0;
                double isoMultiplier = iso / 100.0;
                double apertureValue = double.Parse(aperture);
                double apertureMultiplier = Math.Pow(2.0, -(Math.Log2(apertureValue / 2.8)));
                return normalizedBrightness * isoMultiplier * apertureMultiplier;
            }

            private double MapBrightnessToRotation(double brightness)
            {
                double minAngle = -90;
                double maxAngle = 90;
                brightness = Math.Clamp(brightness, 0, 1);
                return minAngle + (maxAngle - minAngle) * brightness;
            }

            private double CalculateEV(double adjustedBrightness)
            {
                // Simplified: Map brightness (0..1) to EV (-5..20)
                double minEV = -5;
                double maxEV = 20;
                adjustedBrightness = Math.Clamp(adjustedBrightness, 0, 1);
                return minEV + (maxEV - minEV) * adjustedBrightness;
            }

            //public event PropertyChangedEventHandler PropertyChanged;
            public override event PropertyChangedEventHandler? PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }


