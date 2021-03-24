using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PantryGUI.Models;
using Prism.Mvvm;
using Prism.Commands;

namespace PantryGUI.ViewModels
{
    public class AddItemViewModel : BindableBase
    {
        private string _name;
        private int _quantity;
        private string _category;
        private string _barcode;
        private ICommand _cancelCommand;
        private ICommand _turnOffCamera;
        public CameraConnection Camera { get; private set; }
        private string _cameraButtonText;
        private SoundPlayer s;

        public AddItemViewModel()
        {
            Camera = new CameraConnection();
            Camera.CameraOn();
            _cameraButtonText = "Sluk kamera";
            Camera.BarcodeFoundEvent += found;
            s = new SoundPlayer();
        }

        public string Barcode
        {
            get
            {
                return _barcode;
            }
            set
            {
                SetProperty(ref _barcode, value);
            }
        }

        private void found(object sender, BarcodeFoundEventArgs e)
        {
            Barcode = e.Barcode;
            s.Play();
        }

        public string CameraButtonText
        {
            get
            {
                return _cameraButtonText;
            }
            set
            {
                SetProperty(ref _cameraButtonText, value);
            }
        }

        public ICommand TurnOffCamera
        {
            get
            {
                return _turnOffCamera ?? (_turnOffCamera = new DelegateCommand(TurnOffCamHandler));
            }
        }

        private void TurnOffCamHandler()
        {
            if (CameraButtonText == "Sluk kamera")
            {
                CameraButtonText = "Sluk kamera";
                Camera.CameraOff();
            }
            else
            {
                CameraButtonText = "Tænd kamera";
                Camera.CameraOn();
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelHandler));
            }
        }

        private void CancelHandler()
        {
            Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
        }

        public void ItemNotFound()
        {
            Console.WriteLine("sadf");
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                SetProperty(ref _quantity, value);
            }
        }

        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                SetProperty(ref _category, value);
            }
        }
    }
}
