using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace PantryGUI.Models
{
    public class Items : BindableBase, INotifyDataErrorInfo
    {
        private string _name;
        private int _quantity;
        private DateTime _date;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    List<string> errors = new List<string>
                    {
                        "Name is mandatory."
                    };
                    SetErrors("Name", errors);
                }
                else
                {
                    var isValid = true;
                    foreach (var c in value)
                        if (!char.IsLetter(c))
                        {
                            List<string> errors = new List<string>
                            {
                                "Name can only contain letters!"
                            };
                            SetErrors("Name", errors);
                            isValid = false;
                            break;
                        }

                    if (isValid)
                        ClearErrors("Name");
                }
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
                if (value < 0)
                {
                    List<string> errors = new List<string>
                    {
                        "Age cannot be negative."
                    };
                    SetErrors("Age", errors);
                }
                else
                {
                    ClearErrors("Age");
                }
                SetProperty(ref _quantity, value);
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                SetProperty(ref _date, value);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        #region INotifyDataErrorInfo implementation
        public bool HasErrors
        {
            get
            {
                // Indicate whether the entire object is error-free.
                return (errors.Count > 0);
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                // Provide all the error collections.
                return (errors.Values);
            }
            else
            {
                // Provice the error collection for the requested property
                // (if it has errors).
                if (errors.ContainsKey(propertyName))
                {
                    return (errors[propertyName]);
                }
                else
                {
                    return null;
                }
            }
        }

        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        private void SetErrors(string propertyName, List<string> propertyErrors)
        {
            // Clear any errors that already exist for this property.
            errors.Remove(propertyName);
            // Add the list collection for the specified property.
            errors.Add(propertyName, propertyErrors);
            // Raise the error-notification event.
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void ClearErrors(string propertyName)
        {
            // Remove the error list for this property.
            errors.Remove(propertyName);
            // Raise the error-notification event.
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion
    }
}
