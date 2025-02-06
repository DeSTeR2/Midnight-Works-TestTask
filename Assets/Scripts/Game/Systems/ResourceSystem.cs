using InteractObjects;
using Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomSystems
{
    public class ResourceSystem : MonoBehaviour
    {
        [SerializeField] List<ResourceConfig> resources;
        [SerializeField] int spawnResourseNumberOnStart = 2;
        [SerializeField] ObjectPlace storage;

        Dictionary<ResourceType, ObjectPool<InteractObject>> resourcesDictionary;
        Dictionary<ResourceType, Queue<InteractObject>> resourcesQueue;

        public static ResourceSystem instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            resourcesQueue = new();
            resourcesDictionary = new();
            resources.Sort(new ResourceComparer());

            int resourseNumber = Enum.GetNames(typeof(ResourceType)).Length;
            for (int i = 0; i < resourseNumber; i++)
            {
                InteractObject resource = resources[i].resource;
                ResourceType resourseType = (ResourceType)i;

                ObjectPool<InteractObject> objectPool = new ObjectPool<InteractObject>(resource);
                Queue<InteractObject> queue = new();

                for (int j = 0; j < spawnResourseNumberOnStart; j++)
                {
                    InteractObject res = Instantiate(resource);
                    objectPool.AddObject(res);
                    res.gameObject.SetActive(false);

                    queue.Enqueue(res);
                }

                resourcesQueue[resourseType] = queue;
                resourcesDictionary[resourseType] = objectPool;
                objectPool.OnPoolIsEmpty += SpawnObject;
            }
        }

        public InteractObject RequestSpawnResource(ResourceType resourceType, Vector3 position, Quaternion rotation, bool isOnFloor)
        {
            InteractObject rec = resourcesDictionary[resourceType].GetObject();
            rec.transform.position = position;
            rec.transform.rotation = rotation;

            rec.gameObject.SetActive(true);

            resourcesQueue[resourceType].Enqueue(rec);
            return rec;
        }

        private void SpawnObject(ObjectPool<InteractObject> objectPool)
        {
            ResourceType resType = objectPool.Object.ResourceType;
            InteractObject rec = Instantiate(objectPool.Object);
            objectPool.AddObject(rec);
            resourcesQueue[resType].Enqueue(rec);
        }

        public void BackObject(InteractObject resource)
        {
            resource.gameObject.SetActive(false);
            ResourceType type = resource.ResourceType;
            resourcesDictionary[type].AddObject(resource);
        }

        public async Task<Vector3> GetResourcePosition(ResourceType resourceType)
        {
            Queue<InteractObject> objects = resourcesQueue[resourceType];

            while (true)
            {
                if (objects.Count != 0)
                {
                    InteractObject obj = objects.Dequeue();
                    if (obj.gameObject.activeInHierarchy && obj.GetComponent<BoxCollider>().enabled == true)
                    {
                        return obj.transform.position;
                    }
                }

                await Task.Delay(1);
            }
        }

        public Vector3 GetStoragePosition() => storage.transform.position;
    }
    public class ResourceComparer : IComparer<ResourceConfig>
    {
        public int Compare(ResourceConfig x, ResourceConfig y)
        {
            int xRes = (int)x.resourceType;
            int yRes = (int)y.resourceType;

            if (xRes > yRes) return 1;
            if (xRes < yRes) return -1;
            return 0;
        }

    }
}