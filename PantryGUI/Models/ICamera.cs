using System;

namespace PantryGUI
{
    public class BarcodeFoundEventArgs : EventArgs
    {
        public string Barcode { get; set; }
    }
   public interface ICamera
    {
        event EventHandler<BarcodeFoundEventArgs> BarcodeFoundEvent;
        void CameraOn();

        void CameraOff();
    }
}
