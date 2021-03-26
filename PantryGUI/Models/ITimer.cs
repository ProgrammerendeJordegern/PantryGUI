namespace PantryGUI.Models
{
    interface ITimer
    {
        void Enable();
        void Disable();
        System.Timers.Timer GetTimer();
    }
}
