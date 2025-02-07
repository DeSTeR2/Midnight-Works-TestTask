using InteractObjects;
using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(fileName = "ResourceConfig", menuName = "Resources/Config")]
    public class ResourceConfig : ScriptableObject
    {
        public InteractObject resource;
        public ResourceType resourceType;
        public int price;
    }
}