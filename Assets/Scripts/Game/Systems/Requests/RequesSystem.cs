using System.Collections.Generic;
using UnityEngine;
using RequestManagment;
using Character.Worker;
using System.Threading.Tasks;
using System.Text;
using System;

namespace CustomSystems
{
    public static class RequesSystem {
        static List<Request> requests = new();

        static bool gamePlay = false;

        public static Action OnUpdateRequests;

        public static void InitSystem()
        {
            Request.OnRequestCreate += RequestCreated;
            gamePlay = true;
            StartPerfoming();
        }

        public static void Delete()
        {
            Request.OnRequestCreate -= RequestCreated;
            gamePlay = false;
        }

        private static async void StartPerfoming()
        {
            while (gamePlay)
            {
                if (requests.Count != 0)
                {
                    await PerformeRequest();
                }
                else
                {
                    await Task.Delay(1000);
                    OnUpdateRequests?.Invoke();
                }
            }
        }
  
        private static void RequestCreated(Request request)
        {
            requests.Add(request);
            requests.Sort(new RequestComparer());
        }

        private static async Task PerformeRequest()
        {

            DeliveryWorker worker = await WorkerSystem.instance.GetWorker();
            Request request = null;
            while (request == null)
            {
                request = requests[0];
                requests.RemoveAt(0);
            }
            request.PerfomRequest(worker);
        }

        public static string Requests()
        {
            var sb = new StringBuilder();

            foreach (Request request in requests)
            {
                sb.AppendLine(request.ToString());
            }

            return sb.ToString();
        }
    }

    public class RequestComparer : IComparer<Request>
    {
        public int Compare(Request x, Request y)
        {
            if (x.priority == y.priority) return 0;
            if (x.priority < y.priority) return 1;
            return -1;
        }
    }
}