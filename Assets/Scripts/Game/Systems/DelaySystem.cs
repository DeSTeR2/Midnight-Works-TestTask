using System.Threading.Tasks;

namespace CustomSystems
{
    public static class DelaySystem
    {
        public delegate void Function();

        public static async void DelayFunction(Function function, float delay)
        {
            await Task.Delay((int)(delay * 1000));
            function?.Invoke();
        }
    }
}