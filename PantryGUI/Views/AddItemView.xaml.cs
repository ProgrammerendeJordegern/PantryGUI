using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PantryGUI.ViewModels;

namespace PantryGUI.Views
{
    /// <summary>
    /// Interaction logic for AddItemView.xaml
    /// </summary>
    public partial class AddItemView : Window
    {
        private AddItemViewModel addItemViewModel;
        public AddItemView()
        {
            InitializeComponent();
            addItemViewModel = new AddItemViewModel();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            addItemViewModel.Camera.CameraOff();
        }

        //private void Close(object sender, ContextMenuEventArgs e)
        //{
        //    addItemViewModel.Camera.CameraOff();
        //}
    }
}
