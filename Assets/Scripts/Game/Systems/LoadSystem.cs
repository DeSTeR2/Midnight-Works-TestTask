using System;

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

        public void UpdateLoadTime(float loadTime) { 
            this.loadTime = loadTime;
            tickNumber = (int)(loadTime / tickTime);
        }

        public async void Load()
        {
            float loadTime = this.loadTime;
            int tickNumber = this.tickNumber;
            for (int i = 0; i <= tickNumber; i++) {
                float loadedPercent = ((i + 1) * tickTime) / loadTime;
                await DelaySystem.DelayFunction(delegate { 
                    OnTickEnd?.Invoke(loadedPercent);
                },  tickTime);
            }
        }
    }
}