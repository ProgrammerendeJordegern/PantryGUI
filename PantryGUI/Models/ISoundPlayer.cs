namespace PantryGUI
{
    interface ISoundPlayer
    {
        void Play();
        bool Mute { get; set; }
    }
}
