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
        private ICommand _cancelCommand;
        private ICommand 

        public ICamera Camera;
        public AddItemViewModel()
        {
            Camera = new CameraConnection();
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
