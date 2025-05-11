using CommunityToolkit.Mvvm.Input;
using Location.Photography.Shared.ViewModels.Interfaces;
using Locations.Core.Shared.ViewModels;
using Microsoft.Maui.Graphics;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Location.Photography.Shared.ViewModels
{
    public partial class SceneEvaluationViewModel : ViewModelBase, ISceneEvaluation
    {
        #region Fields
        private byte[] RedComponents;
        private byte[] GreenComponents;
        private byte[] BlueComponents;
        private byte[] ContrastValues;
        private int[] RedHistogram;
        private int[] GreenHistogram;
        private int[] BlueHistogram;
        private int[] ContrastHistogram;
        private int TotalPixels;
        private string _redHistogram = string.Empty;
        private string _greenHistogram = string.Empty;
        private string _blueHistogram = string.Empty;
        private string _contrastHistogram = string.Empty;
        private bool _isProcessing;
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<OperationErrorEventArgs>? ErrorOccurred;
        #endregion

        #region Properties
        public string RedHistogramImage
        {
            get => _redHistogram;
            set
            {
                if (_redHistogram != value)
                {
                    _redHistogram = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BlueHistogramImage
        {
            get => _blueHistogram;
            set
            {
                if (_blueHistogram != value)
                {
                    _blueHistogram = value;
                    OnPropertyChanged();
                }
            }
        }

        public string GreenHistogramImage
        {
            get => _greenHistogram;
            set
            {
                if (_greenHistogram != value)
                {
                    _greenHistogram = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ContrastHistogramImage
        {
            get => _contrastHistogram;
            set
            {
                if (_contrastHistogram != value)
                {
                    _contrastHistogram = value;
                    OnPropertyChanged();
                }
            }
        }

        // Interface compatibility properties
        public bool VmIsBusy
        {
            get => IsBusy;
            set => IsBusy = value;
        }

        public string VmErrorMessage
        {
            get => ErrorMessage;
            set => ErrorMessage = value;
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                if (_isProcessing != value)
                {
                    _isProcessing = value;
                    OnPropertyChanged();
                    ((AsyncRelayCommand)EvaluateCommand).NotifyCanExecuteChanged();
                }
            }
        }
        #endregion

        #region Commands
        public ICommand EvaluateCommand { get; }
        #endregion

        #region Constructors
        public SceneEvaluationViewModel()
        {
            // Initialize histograms
            RedComponents = new byte[256];
            GreenComponents = new byte[256];
            BlueComponents = new byte[256];
            ContrastValues = new byte[256];
            RedHistogram = new int[256];
            GreenHistogram = new int[256];
            BlueHistogram = new int[256];
            ContrastHistogram = new int[256];

            // Initialize commands
            EvaluateCommand = new AsyncRelayCommand(EvaluateSceneAsync, () => !IsProcessing);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Evaluates the scene by capturing a photo and generating histograms
        /// </summary>
        private async Task EvaluateSceneAsync()
        {
            try
            {
                IsBusy = true;
                IsProcessing = true;
                ErrorMessage = string.Empty;

                await GetImage();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error evaluating scene: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    ErrorMessage,
                    ex));
            }
            finally
            {
                IsBusy = false;
                IsProcessing = false;
            }
        }

        /// <summary>
        /// Captures an image and processes it to generate histograms
        /// </summary>
        private async Task<int> GetImage()
        {
            string path = string.Empty;
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = null;

                try
                {
                    photo = await MediaPicker.Default.CapturePhotoAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Error capturing photo: {ex.Message}";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.MediaService,
                        ErrorMessage,
                        ex));
                    return -1;
                }

                if (photo != null)
                {
                    await Task.Run(async () =>
                    {
                        try
                        {
                            // Save the file into local storage
                            string localFilePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, photo.FileName);
                            DirectoryInfo di = new DirectoryInfo(Microsoft.Maui.Storage.FileSystem.AppDataDirectory);
                            var files = di.GetFiles();

                            foreach (var file in files)
                            {
                                if (file.Extension == ".jpg")
                                {
                                    file.Delete();
                                }
                            }

                            using Stream sourceStream = await photo.OpenReadAsync();
                            using FileStream localFileStream = File.OpenWrite(localFilePath);
                            path = localFilePath;
                            await sourceStream.CopyToAsync(localFileStream);

                            double[] redHistogram = new double[256];
                            double[] greenHistogram = new double[256];
                            double[] blueHistogram = new double[256];
                            double[] contrastHistogram = new double[256];

                            using (var bitmap = SKBitmap.Decode(path))
                            {
                                int totalPixels = bitmap.Width * bitmap.Height;

                                for (int y = 0; y < bitmap.Height; y++)
                                {
                                    for (int x = 0; x < bitmap.Width; x++)
                                    {
                                        SKColor color = bitmap.GetPixel(x, y);
                                        redHistogram[color.Red]++;
                                        greenHistogram[color.Green]++;
                                        blueHistogram[color.Blue]++;

                                        // Contrast calculation (Luminance Approximation)
                                        int contrast = (int)(0.299 * color.Red + 0.587 * color.Green + 0.114 * color.Blue);
                                        contrastHistogram[contrast]++;
                                    }
                                }

                                // Normalize histograms to percentage values (0-1 range)
                                NormalizeHistogram(redHistogram, totalPixels);
                                NormalizeHistogram(greenHistogram, totalPixels);
                                NormalizeHistogram(blueHistogram, totalPixels);
                                NormalizeHistogram(contrastHistogram, totalPixels);
                            }

                            string redPath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "red.png");
                            string bluePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "blue.png");
                            string greenPath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "green.png");
                            string contrastPath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "contrast.png");

                            // Generate histogram images
                            string redResult = GenerateHistogramImage(redPath, redHistogram, SKColors.Red);
                            string greenResult = GenerateHistogramImage(greenPath, greenHistogram, SKColors.Green);
                            string blueResult = GenerateHistogramImage(bluePath, blueHistogram, SKColors.Blue);
                            string contrastResult = GenerateHistogramImage(contrastPath, contrastHistogram, SKColors.Black);

                            // Update the UI properties on the main thread
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                RedHistogramImage = redResult;
                                GreenHistogramImage = greenResult;
                                BlueHistogramImage = blueResult;
                                ContrastHistogramImage = contrastResult;
                            });
                        }
                        catch (Exception ex)
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                ErrorMessage = $"Error processing image: {ex.Message}";
                                OnErrorOccurred(new OperationErrorEventArgs(
                                    OperationErrorSource.Unknown,
                                    ErrorMessage,
                                    ex));
                            });
                        }
                    });
                }
            }
            else
            {
                ErrorMessage = "Camera capture is not supported on this device.";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.MediaService,
                    ErrorMessage));
            }

            return 0;
        }

        /// <summary>
        /// Normalizes histogram data to the range [0,1]
        /// </summary>
        private static void NormalizeHistogram(double[] histogram, int totalPixels)
        {
            double maxValue = 0;
            for (int i = 0; i < histogram.Length; i++)
            {
                histogram[i] /= totalPixels;  // Normalize to [0,1]
                if (histogram[i] > maxValue)
                    maxValue = histogram[i]; // Get max value
            }

            // Scale values so the highest value is 100% of the image height
            if (maxValue > 0)
            {
                for (int i = 0; i < histogram.Length; i++)
                {
                    histogram[i] /= maxValue;
                }
            }
        }

        /// <summary>
        /// Generates a histogram image and saves it to the specified file path
        /// </summary>
        private static string GenerateHistogramImage(string filePath, double[] histodata, SKColor color)
        {
            int width = 512;  // Histogram image width
            int height = 256; // Histogram image height
            int margin = 10;  // Margin for better visibility

            using (var surface = SKSurface.Create(new SKImageInfo(width, height)))
            {
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                // Draw Axes
                using (var axisPaint = new SKPaint { Color = SKColors.Black, StrokeWidth = 2, IsAntialias = true })
                {
                    canvas.DrawLine(margin, height - margin, width - margin, height - margin, axisPaint); // X-axis
                    canvas.DrawLine(margin, height - margin, margin, margin, axisPaint); // Y-axis
                }

                // Draw Histogram
                DrawHistogramLine(canvas, histodata, color, width, height, margin);

                // Save Image
                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(filePath))
                {
                    data.SaveTo(stream);
                }
                return filePath;
            }
        }

        /// <summary>
        /// Draws a histogram line on the canvas
        /// </summary>
        private static void DrawHistogramLine(SKCanvas canvas, double[] histogram, SKColor color, int width, int height, int margin)
        {
            int graphWidth = width - (2 * margin);
            int graphHeight = height - (2 * margin);

            using (var paint = new SKPaint
            {
                Color = color,
                StrokeWidth = 2,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            })
            {
                for (int i = 1; i < histogram.Length; i++)
                {
                    float x1 = margin + ((i - 1) * (graphWidth / 256f));
                    float y1 = height - margin - (float)(histogram[i - 1] * graphHeight);
                    float x2 = margin + (i * (graphWidth / 256f));
                    float y2 = height - margin - (float)(histogram[i] * graphHeight);

                    canvas.DrawLine(x1, y1, x2, y2, paint);
                }
            }
        }

        /// <summary>
        /// Raise the error event
        /// </summary>
        protected virtual void OnErrorOccurred(OperationErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}