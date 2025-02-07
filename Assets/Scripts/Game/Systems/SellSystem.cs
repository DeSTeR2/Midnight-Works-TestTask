using InteractObjects.Work;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomSystems { 
    public static class SellSystem
    {
        static HashSet<ResourceType> availableResources = new();

        public static Action<ResourceType> OnNewSellRequest;

        public static void InitSystem()
        {
            MachineWork.OnAwailableResource += AddResource;
        }

        private static void AddResource(ResourceType resourceType) {
            availableResources.Add(resourceType);   
        }

        public static ResourceType CreateSellRequest()
        {
            Random rng = new Random();
            int index = rng.Next(0, availableResources.Count);

            ResourceType type = availableResources.ToList()[index];
            return type;
        }

        public static void Delete()
        {
            MachineWork.OnAwailableResource += AddResource;
        }
    }
}