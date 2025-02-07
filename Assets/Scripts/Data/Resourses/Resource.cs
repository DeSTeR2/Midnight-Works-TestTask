using UnityEngine;

namespace Resources
{
    public class Resource
    {
        ResourceType resourseType;
        ResourceConfig config;

        public Resource(ResourceType resourseType, ResourceConfig config)
        {
            this.resourseType = resourseType;
            this.config = config;
        }

        public ResourceType ResourseType { get => resourseType; }
        public int Price { get => config.price; }

        public void SetConfig(ResourceConfig config) => this.config = config;
    }

    public enum ResourceType
    {
        Wood,
        Plank,
        Box, 
        Chair,
        None
    }
}