using System;
using Android.Content;
using Android.Hardware;
using aa=Android.App;
using Location.Core.Platforms.Android.Interface;
using Microsoft.Maui.Controls;
using Location.Core.Platforms.Android.Implementation;
[assembly: Dependency(typeof(AmbientLightSensorService))]
namespace Location.Core.Platforms.Android.Implementation
{
    public class AmbientLightSensorService : Java.Lang.Object, IAmbientLightSensorService, ISensorEventListener
    {
        private SensorManager _sensorManager;
        private Sensor _lightSensor;
        public bool IsRunning { get; set; } = false;
        // Event to notify the UI when the light level changes
        public event EventHandler<float> LightLevelChanged;

        public AmbientLightSensorService()
        {
            _sensorManager = (SensorManager)aa.Application.Context.GetSystemService(Context.SensorService);
            _lightSensor = _sensorManager.GetDefaultSensor(SensorType.Light);
            var sensors = _sensorManager.GetSensorList(SensorType.Light);
            
        }

        public void StartListening()
        {
            IsRunning = true;
            if (_lightSensor != null)
            {
                _sensorManager.RegisterListener(this, _lightSensor, SensorDelay.Normal);
            }
        }

        public void StopListening()
        {
            IsRunning = false;
            _sensorManager.UnregisterListener(this);
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type == SensorType.Light)
            {
                float lux = e.Values[0];
                System.Diagnostics.Debug.WriteLine($"Ambient light level: {lux} lux");

                // Raise the event to notify the UI about the light level
                LightLevelChanged?.Invoke(this, lux);
            }
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy) { }
    }
}
