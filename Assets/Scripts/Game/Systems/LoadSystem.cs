using System;
using System.Threading.Tasks;

namespace CustomSystems
{
    public class LoadSystem
    {
        float loadTime;
        float tickTime;

        int tickNumber;

        public Action<float> OnTickEnd;
        
        public LoadSystem(float loadTime, float tickTime) {
            this.loadTime = loadTime;
            this.tickTime = tickTime;
            tickNumber = (int)(loadTime / tickTime);
        }

        public async void Load()
        {
            for (int i = 0; i <= tickNumber; i++) {
                float loadedPercent = ((i + 1) * tickTime) / loadTime;
                await DelaySystem.DelayFunction(delegate { 
                    OnTickEnd?.Invoke(loadedPercent);
                },  tickTime);
            }
        }
    }
}