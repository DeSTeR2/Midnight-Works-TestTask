using System;
using System.Threading;
using System.Threading.Tasks;

namespace CustomSystems
{
    public class LoadSystem
    {
        float loadTime;
        float tickTime;
        int tickNumber;
        CancellationTokenSource cancellationTokenSource;

        public Action<float> OnTickEnd;

        public LoadSystem(float loadTime, float tickTime)
        {
            this.loadTime = loadTime;
            this.tickTime = tickTime;
            tickNumber = (int)(loadTime / tickTime);
        }

        public void UpdateLoadTime(float loadTime)
        {
            this.loadTime = loadTime;
            tickNumber = (int)(loadTime / tickTime);
        }

        public void Stop()
        {
            cancellationTokenSource?.Cancel();
        }

        public async Task Load()
        {
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            float currentLoadTime = loadTime;
            int currentTickNumber = tickNumber;

            for (int i = 0; i <= currentTickNumber; i++)
            {
                token.ThrowIfCancellationRequested();
                float loadedPercent = ((i + 1) * tickTime) / currentLoadTime;

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(tickTime), token);
                    OnTickEnd?.Invoke(loadedPercent);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}