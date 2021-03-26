using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private BackendConnection _backendConnection;
        private ICommand _cancelCommand;
        private ICommand _okCommand;
        private ICommand _turnOffCamera;
        public CameraConnection Camera { get; private set; }
        private string _cameraButtonText;
        private SoundPlayer _soundPlayer;
        private int _cameraListIndex;

        public enum CameraState { CameraOn, CameraOff }
        private CameraState _stateForCamera;
        
        public ObservableCollection<string> CameraList { get; private set; }

        public AddItemViewModel()
        {
            Camera = new CameraConnection();
            Camera.CameraOn();
            _cameraButtonText = "Sluk kamera";
            Camera.BarcodeFoundEvent += found;
            _soundPlayer = new SoundPlayer();
            _stateForCamera = CameraState.CameraOn;
            CameraList = new ObservableCollection<string>();
            CameraList = Camera.CamerasList;
            _backendConnection = new BackendConnection();
        }

        public int CameraListIndex
        {
            get
            {
                return _cameraListIndex;
            }
            set
            {
                Camera.SetCameraListIndex(value);
                SetProperty(ref _cameraListIndex, value);
            }
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
            _soundPlayer.Play();
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
            if (_stateForCamera == CameraState.CameraOn)
            {
                _stateForCamera = CameraState.CameraOff;
                CameraButtonText = "Tænd kamera";
                Camera.CameraOff();
                Camera.CameraFeed = null;
            }
            else
            {
                _stateForCamera = CameraState.CameraOn;
                CameraButtonText = "Sluk kamera";
                Camera.CameraOn();
            }
        }

        public ICommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new DelegateCommand(OkHandler));
            }
        }

        private void OkHandler()
        {
            _backendConnection.SetNewItem("Test", "Test", "Test");
            Camera.CameraOff();
            Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
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
            Camera.CameraOff();
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
