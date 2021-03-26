﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using ZXing;
using AForge.Video.DirectShow;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using Prism.Mvvm;

namespace PantryGUI.Models
{
    public class CameraConnection : ICamera
    {

        //opret et interfase til en lyd afspiller
        //Klassen kunne indeholde en bool som beskiver om den skal være muted. 
        //eller skal den afspille lyd når der kommer et event. 
        private FilterInfoCollection _filterInfoCollection;
        private VideoCaptureDevice _videoCaptureDevice;
        private List<string> _camerasList;
        private BitmapImage _cameraFeed;

        public event EventHandler<BarcodeFoundEventArgs> BarcodeFoundEvent;

        public CameraConnection()
        {
            _camerasList = new List<string>();
            _filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo device in _filterInfoCollection)
            {
                _camerasList.Add(device.Name);
            }
        }

        public BitmapImage GetCameraFeed()
        {
            return _cameraFeed;
        }

        public List<string> GetCameraList()
        {
            return _camerasList;
        }

        public void CameraOn()
        {
            _videoCaptureDevice = new VideoCaptureDevice(_filterInfoCollection[0].MonikerString);
            _videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            _videoCaptureDevice.Start();
        }


        private void VideoCaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            BarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bitmap);

            if (result != null)
            {
                BarcodeFound(new BarcodeFoundEventArgs { Barcode = result.ToString() });
            }

            _cameraFeed = Convert(bitmap);
            _cameraFeed.Freeze();
        }

        private BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }


        //Lav et interfase til den 
        //tilføj et event i interfaset:
        //event BarcodeFound ......

        //lav en eventargs klasse som indeholder en string
        //handleren kan så implenteres i view modellen

        //Den skal have on /off funktion

        public void CameraOff()
        {
            if (_videoCaptureDevice != null)
            {
                if (_videoCaptureDevice.IsRunning)
                {
                    _videoCaptureDevice.Stop();
                }
            }
        }

        protected virtual void BarcodeFound(BarcodeFoundEventArgs e)
        {
            BarcodeFoundEvent?.Invoke(this, e);
        }

        public string GetBarcode()
        {
            return "barcode";
        }

        
    }
}
