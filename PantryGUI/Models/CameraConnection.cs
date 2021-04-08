﻿using System;
using System.Collections.ObjectModel;
using AForge.Video.DirectShow;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Prism.Mvvm;

namespace PantryGUI.Models
{
    public class CameraConnection : BindableBase, ICamera
    {
        private FilterInfoCollection _filterInfoCollection;
        private VideoCaptureDevice _videoCaptureDevice;
        public ObservableCollection<string> CamerasList { get; private set; }
        private int _cameraListIndex;
        private BitmapImage _cameraFeed;
        private ReadBarcode _reader;
        private ITimer<Timer> _timer;

        public event EventHandler<BarcodeFoundEventArgs> BarcodeFoundEvent;

        private static readonly object padlock = new object();
        private static CameraConnection _instance = null;

        //Thread Safety Singleton
        public static CameraConnection Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new CameraConnection();
                    }
                    return _instance;
                }
            }
        }

        private CameraConnection()
        {
            _cameraListIndex = 0;
            _reader = new ReadBarcode();
            _timer = new TimerClock(2000);
            _timer.GetTimer().Elapsed += new ElapsedEventHandler(TimeHandler);
            CamerasList = new ObservableCollection<string>();
            _filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo device in _filterInfoCollection)
            {
                CamerasList.Add(device.Name);
            }
        }

        public BitmapImage CameraFeed
        {
            get
            {
                return _cameraFeed;
            }
            set
            {
                SetProperty(ref _cameraFeed, value);
            }
        }

        public void CameraOn()
        {
            _videoCaptureDevice = new VideoCaptureDevice(_filterInfoCollection[_cameraListIndex].MonikerString);
            _videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            _videoCaptureDevice.Start();
        }


        public void SetCameraListIndex(int index)
        {
            _cameraListIndex = index;
        }

        private void VideoCaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

            string barcode = _reader.GetBarcode(bitmap);

            if (barcode != null)
            {
                BarcodeFound(new BarcodeFoundEventArgs { Barcode = barcode });
                _reader.Deactivate();
                _timer.Enable();
            }

            Application.Current.Dispatcher.BeginInvoke(new Action(() => { CameraFeed = Convert(bitmap); }));
            Application.Current.Dispatcher.BeginInvoke(new Action(() => { CameraFeed.Freeze(); }));
            
        }

        private void TimeHandler(object source, ElapsedEventArgs e)
        {
            _reader.Activate();
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
    }
}
