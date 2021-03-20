using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PantryGUI.Models
{
    public class CameraConnection
    {

        //Lav et interfase til den 
        //tilføj et event i interfaset:
        //event BarcodeFound ......

        //lav en eventargs klasse som indeholder en string
        //handleren kan så implenteres i view modellen

        //Den skal have on /off funktion


        public string GetBarcode()
        {
            return "barcode";
        }
    }
}
