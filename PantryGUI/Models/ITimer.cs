namespace PantryGUI.Models
{
    interface ITimer<T>
    {
        void Enable();
        void Disable();
        T GetTimer();
    }
}
