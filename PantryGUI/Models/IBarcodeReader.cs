using System.Drawing;

namespace PantryGUI.Models
{
    interface IBarcodeReader
    {
        string GetBarcode(Bitmap image);
        void Deactivate();
        void Activate();
    } 
    
}
