using Android.Content;
using Android.Hardware.Camera2;
using Android.Util;
using Java.Lang;
using SkiaSharp;
using System;
using System.Threading.Tasks;
using Android.Graphics;
using ag = Android.Graphics;
using static Android.Views.TextureView;

namespace Location.Core.Platforms.Android
{
    public class CameraFrameService
    {
        private CameraCaptureSession _cameraCaptureSession;
        private CameraDevice _cameraDevice;
        private CameraCharacteristics _cameraCharacteristics;
        private CaptureRequest.Builder _captureRequestBuilder;
        private ISurfaceTextureListener _listener;

        public event Action<byte[]> FrameCaptured;

        public CameraFrameService(ISurfaceTextureListener listener)
        {
            _listener = listener;
        }

        public async Task StartCameraAsync()
        {
            var cameraManager = (CameraManager)Android.MainApplication.Context.GetSystemService(Context.CameraService);
            string cameraId = GetBackCameraId(cameraManager);

            if (cameraId != null)
            {
                _cameraCharacteristics = GetCameraCharacteristics(cameraManager, cameraId);  // Synchronous call here
                _cameraDevice = await OpenCameraAsync(cameraManager, cameraId);
                if (_cameraDevice != null)
                {
                    StartPreviewSession();
                }
            }
        }

        private string GetBackCameraId(CameraManager cameraManager)
        {
            for (int i = 0; i < cameraManager.GetCameraIdList().Length; i++)
            {
                var characteristics = cameraManager.GetCameraCharacteristics(cameraManager.GetCameraIdList()[i]);
                var facing = (Integer)characteristics.Get(CameraCharacteristics.LensFacing);
                if (facing == (Integer)((int)LensFacing.Back))
                {
                    return cameraManager.GetCameraIdList()[i];
                }
            }
            return null;
        }

        private async Task<CameraDevice> OpenCameraAsync(CameraManager cameraManager, string cameraId)
        {
            var tcs = new TaskCompletionSource<CameraDevice>();
            cameraManager.OpenCamera(cameraId, new CameraStateListener(tcs), null);
            return await tcs.Task;
        }

        private CameraCharacteristics GetCameraCharacteristics(CameraManager cameraManager, string cameraId)
        {
            return cameraManager.GetCameraCharacteristics(cameraId);
        }

        private void StartPreviewSession()
        {
            // Add logic to start previewing the camera frame here, and notify the listener with frame data
        }

        // Helper function to convert the image data into byte array
        private byte[] ConvertImageToBytes(ag.Bitmap image)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                image.Compress(ag.Bitmap.CompressFormat.Png, 100, stream);
                return stream.ToArray();
            }
        }

    }
    class CameraStateListener : CameraDevice.StateCallback
    {
        private readonly TaskCompletionSource<CameraDevice> _tcs;

        public CameraStateListener(TaskCompletionSource<CameraDevice> tcs)
        {
            _tcs = tcs;
        }

        public override void OnOpened(CameraDevice camera)
        {
            // Successfully opened camera
            _tcs.SetResult(camera);
        }

        public override void OnDisconnected(CameraDevice camera)
        {
            // Camera was disconnected
            camera.Close();
        }

        public override void OnError(CameraDevice camera, CameraError error)
        {
            // Handle error
            //Log.Error("CameraFrameService", $"Camera error: {error}");
            //_tcs.SetException(new Exception($"Camera error: {error}"));
        }
    }
}
