using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PantryGUI.Models;
using Prism.Commands;


namespace PantryGUI.ViewModels
{
    class MainWindowViewModel
    {
        public tempUser t1
        {
            get;
            set;
        }
        
        public MainWindowViewModel()
        {
            t1 = new tempUser("Kristian");
        }

        ICommand _addItemCommand;

        public ICommand AddItemCommand
        {
            get { return _addItemCommand ?? (_addItemCommand = new DelegateCommand(AddItemExecute)); }
        }

        void AddItemExecute()
        {
            MessageBox.Show("This is where u add an item...");
        }


    }
}
