using System;
using System.Threading.Tasks;

namespace CustomSystems
{
    public class LoadSystem
    {
        float loadTime;
        float tickTime;

        float loadedPercent;
        
        public Action<float> OnTickEnd;
        

        public LoadSystem(float loadTime, float tickTime) {
            this.loadTime = loadTime;
            this.tickTime = tickTime;
        }

        public async void Load()
        {
            int tickNumber = (int)(loadTime / tickTime);
            for (int i = 0; i <= tickNumber; i++) {
                loadedPercent = ((i + 1) * tickTime) / loadTime;
                await DelaySystem.DelayFunction(delegate { 
                    OnTickEnd?.Invoke(loadedPercent);
                },  tickTime);
            }
        }
    }
}