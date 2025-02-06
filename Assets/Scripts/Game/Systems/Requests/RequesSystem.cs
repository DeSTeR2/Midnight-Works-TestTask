using System.Collections.Generic;
using UnityEngine;
using Request;
using Character.Worker;
using System.Threading.Tasks;
using System.Text;

namespace CustomSystems
{
    public static class RequesSystem {
        static Queue<Request.Request> requests = new();

        static bool gamePlay = false;

        public static void InitSystem()
        {
            Request.Request.OnRequestCreate += RequestCreated;
            gamePlay = true;
            StartPerfoming();
        }

        public static void Delete()
        {
            Request.Request.OnRequestCreate -= RequestCreated;
            gamePlay = false;
        }

        private static async void StartPerfoming()
        {
/*            while (gamePlay)
            {
                if (requests.Count != 0)
                {
                    await PerformeRequest();
                } else
                {
                    await Task.Delay(2000);
                }
            }*/
        }
  
        private static void RequestCreated(Request.Request request)
        {
            requests.Enqueue(request);
            PerformeRequest();
        }

        private static async Task PerformeRequest()
        {
            Request.Request request = requests.Dequeue();
            DeliveryWorker worker = await WorkerSystem.instance.GetWorker();
            request.PerfomRequest(worker);
        }

        public static string Requests()
        {
            var sb = new StringBuilder();

            foreach (Request.Request request in requests)
            {
                sb.AppendLine(request.ToString() + "\n");
            }

            return sb.ToString();
        }
    }
}