using System;
using System.Collections.ObjectModel;
using AForge.Video.DirectShow;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Windows.Media.Imaging;
using Prism.Mvvm;

namespace PantryGUI.Models
{
    public class CameraConnection : BindableBase, ICamera
    {

        //opret et interfase til en lyd afspiller
        //Klassen kunne indeholde en bool som beskiver om den skal være muted. 
        //eller skal den afspille lyd når der kommer et event. 
        private FilterInfoCollection _filterInfoCollection;
        private VideoCaptureDevice _videoCaptureDevice;
        public ObservableCollection<string> CamerasList { get; private set; }
        private int _cameraListIndex;
        private BitmapImage _cameraFeed;
        private ReadBarcode _reader;
        private TimerClock _timer;

        public event EventHandler<BarcodeFoundEventArgs> BarcodeFoundEvent;

        public CameraConnection()
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

            CameraFeed = Convert(bitmap);
            CameraFeed.Freeze();
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
