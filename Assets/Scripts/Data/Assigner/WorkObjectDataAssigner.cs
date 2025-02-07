using Character.Worker;
using InteractObjects.Work;
using UnityEngine;

namespace Data.Assigner
{
    public class WorkObjectDataAssigner : MonoBehaviour
    {
        [SerializeField] MachineWork workObject;
        [SerializeField] StationaryWorker worker;
        [SerializeField] WorkObjectConfig workConfig;

        private void Start()
        {
            Assign();

            workConfig.OnConfigChanged += Assign;
        }

        private void Assign()
        {
            workObject.WorkTime = workConfig.objectData.workTime;
            workObject.gameObject.SetActive(workConfig.objectData.isActive);
            worker.gameObject.SetActive(workConfig.objectData.isHaveWorker);

            workObject.SetPutPlace(workConfig.objectData.putCapasity);
            workObject.SetTakePlace(workConfig.objectData.takeCapasity);
        }

        private void OnDestroy()
        {
            workConfig.OnConfigChanged -= Assign;
        }
    }
}