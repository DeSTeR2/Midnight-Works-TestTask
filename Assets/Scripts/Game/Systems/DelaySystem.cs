using System.Threading.Tasks;

namespace CustomSystems
{
    public static class DelaySystem
    {
        public delegate void Function();

        public static async Task DelayFunction(Function function, float delay)
        {
            await Task.Delay((int)(delay * 1000));
            try
            {
                function?.Invoke();
            }
            catch { }
        }
    }
}