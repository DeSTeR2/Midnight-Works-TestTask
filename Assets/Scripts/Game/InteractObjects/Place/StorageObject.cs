using CustomSystems;
using Data;
using Resources;
using UnityEngine;

namespace InteractObjects.Place
{
    public class StorageObject : ObjectPlace
    {
        [SerializeField] StorageConfig config;
        public void SetItems(DictionaryCustom<ResourceType, int> store)
        {
            foreach (var item in store)
            {
                for (int i=0; i<item.Value; i++)
                {
                    InteractObject interactObject = ResourceSystem.instance.RequestSpawnResource(item.Key, Vector3.zero, Quaternion.identity, true);
                    base.PutObject(interactObject);
                }
            }
        }

        public override void PutObject(InteractObject interactObject)
        {
            base.PutObject(interactObject);
            config.storageData.store[interactObject.ResourceType]++;
        }

        public override void TookResource(ResourceType resourceType)
        {
            config.storageData.store[resourceType]--;
        }

        public InteractObject GetResourse(ResourceType type)
        {
            try
            {
                InteractObject interactObject = placeStrategy.FindRightResource(type);
                //config.storageData.store[interactObject.ResourceType]--;

                return interactObject;
            }
            catch {
                throw new System.Exception("There is no resource");
            }
        }
    }
}