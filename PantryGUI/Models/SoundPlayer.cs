using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantryGUI.Models
{
    public class SoundPlayer : ISoundPlayer
    {
        public SoundPlayer()
        {
            Mute = false;
        }

        public bool Mute { get; set; }

        public void Play()
        {
            if (Mute == false)
            {
                Console.Beep();
            }
        }
    }
}