using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantryGUI
{
    public class BarcodeFoundEventArgs : EventArgs
    {
        public string Barcode { get; set; }
    }
    interface ICamera
    {
        event EventHandler<BarcodeFoundEventArgs> BarcodeFoundEvent;
    }
}
