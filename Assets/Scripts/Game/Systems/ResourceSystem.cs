using InteractObjects;
using InteractObjects.Place;
using InteractObjects.Work;
using Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomSystems
{
    public class ResourceSystem : MonoBehaviour
    {
        [SerializeField] DictionaryCustom<ResourceType, ResourceConfig> resources;
        [SerializeField] int spawnResourseNumberOnStart = 2;
        [SerializeField] StorageObject storage;

        Dictionary<ResourceType, ObjectPool<InteractObject>> resourcesDictionary;
        Dictionary<ResourceType, Queue<InteractObject>> resourcesQueue;

        public static ResourceSystem instance;

        private void Awake()
        {
            instance = this;
            resources.Synchronize();

            resourcesQueue = new();
            resourcesDictionary = new();

            int resourseNumber = Enum.GetNames(typeof(ResourceType)).Length;
            for (int i = 0; i < resourseNumber; i++)
            {
                ResourceType resourseType = (ResourceType)i;
                if (resourseType != ResourceType.None)
                {

                    InteractObject resource = resources[resourseType].resource;

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
        }

        public int GetResourcePrice(ResourceType type) => resources[type].price;

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
            resource.transform.parent = null;
            resource.transform.rotation = Quaternion.identity;

            ResourceType type = resource.ResourceType;
            resourcesDictionary[type].AddObject(resource);
            BackResourceToQueue(resource);
        }

        public void BackResourceToQueue(InteractObject resource)
        {
            resourcesQueue[resource.ResourceType].Enqueue(resource);
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

        public int GetPrice(ResourceType resourceType) {
            return resources[resourceType].price;
        } 

        public StorageObject GetStorageObject() => storage;
        public Vector3 GetStoragePosition() => storage.transform.position;
    }
}