using Character.Worker;
using InteractObjects.Place;
using InteractObjects.Work;
using UnityEngine;

namespace Data.Assigner
{
    public class StorageDataAssigner : MonoBehaviour
    {
        [SerializeField] StorageObject storage;
        [SerializeField] StorageConfig workConfig;

        private void Start()
        {
            Assign();

            workConfig.OnConfigChanged += Assign;
        }

        private void Assign()
        {
            storage.SetCapability(workConfig.storageData.capability);
        }

        private void OnDestroy()
        {
            workConfig.OnConfigChanged -= Assign;
        }
    }
}