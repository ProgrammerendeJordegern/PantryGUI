using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ??= (new DelegateCommand(CancelHandler));
            }
        }

        private void CancelHandler()
        {

        }



    }
}
