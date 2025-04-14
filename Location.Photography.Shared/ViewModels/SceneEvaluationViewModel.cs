using GalaSoft.MvvmLight;
using Location.Photography.Shared.ViewModels.Interfaces;
using Microsoft.Maui.Graphics;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Location.Photography.Shared.ViewModels
{
    public class SceneEvaluationViewModel : ViewModelBase, ISceneEvaluation
    {
        public override event PropertyChangedEventHandler? PropertyChanged;

        public ICommand Evaluate { get; }
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

        public string RedHistogramImage
        {
            get { return _redHistogram; }
            set
            {
                if (_redHistogram != value)
                {
                    _redHistogram = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RedHistogramImage)));
                }
            }
        }

        public string BlueHistogramImage { get { return _blueHistogram; } set { _blueHistogram = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BlueHistogramImage))); } }
        public string GreenHistogramImage { get { return _greenHistogram; } set { _greenHistogram = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GreenHistogramImage))); } }
        public string ContrastHistogramImage { get { return _contrastHistogram; } set { _contrastHistogram = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContrastHistogramImage))); } }


        public SceneEvaluationViewModel()
        {
            RedComponents = new byte[256];
            GreenComponents = new byte[256];
            BlueComponents = new byte[256];
            ContrastValues = new byte[256];
            RedHistogram = new int[256];
            GreenHistogram = new int[256];
            BlueHistogram = new int[256];
            ContrastHistogram = new int[256];
           // Evaluate = new Command(EvaluateScene);
        }

        public Task<int> EvaluateScene(object obj)
        {
            return GetImage();
        }



        private async Task<int> GetImage()
        {
            string path = string.Empty;
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();


                if (photo != null)
                {
                    await Task.Run(async () =>
                    {

                        // save the file into local storage
                        string localFilePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, photo.FileName);

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
                        string outputPath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "histogram_output.png");

                        _redHistogram = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "red.png");
                        _blueHistogram = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "blue.png");
                        _greenHistogram = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "green.png");
                        _contrastHistogram = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "contrast.png");

                        RedHistogramImage = GenerateHistogramImage(_redHistogram, redHistogram, SKColors.Red);
                        GreenHistogramImage = GenerateHistogramImage(_greenHistogram, greenHistogram, SKColors.Green);
                        BlueHistogramImage = GenerateHistogramImage(_blueHistogram, blueHistogram, SKColors.Blue);
                        ContrastHistogramImage = GenerateHistogramImage(_contrastHistogram, contrastHistogram, SKColors.Black);

                        // GenerateHistogramImage(Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "path.png"), redHistogram, greenHistogram, blueHistogram, contrastHistogram);


                    });
                }
                
            }
            return 69;
        }
        static void NormalizeHistogram(double[] histogram, int totalPixels)
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
        static string GenerateHistogramImage(string filePath, double[] histodata, SkiaSharp.SKColor color)
        {
            int width = 512;  // Histogram image width
            int height = 256; // Histogram image height
            int margin = 10;  // Margin for better visibility
            string localFilePath = filePath;

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

                // Draw Histograms
                SKColor r = color;


                DrawHistogramLine(canvas, histodata, r, width, height, margin);


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

        static void DrawHistogramLine(SKCanvas canvas, double[] histogram, SKColor color, int width, int height, int margin)
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
    }
}



