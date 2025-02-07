using Character.Worker;
using InteractObjects.Work;
using UnityEngine;

namespace Data.Assigner
{
    public class BaseDataAssigner : MonoBehaviour {
        [SerializeField] WorkObject workObject;
        [SerializeField] StationaryWorker worker;
        [SerializeField] BaseWorkConfig workConfig;

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
        }

        private void OnDestroy()
        {
            workConfig.OnConfigChanged -= Assign;
        }
    }
}