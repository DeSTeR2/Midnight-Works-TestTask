using CustomSystems;
using Data;
using System;
using UnityEngine;

namespace InteractObjects.Work
{
    public class WorkObject : MonoBehaviour, IWorkPlace
    {
        [SerializeField] protected float workTime;
        [SerializeField] protected WorkStatus workStatus;
        [SerializeField] protected BaseWorkConfig workConfig;
        [SerializeField] protected bool haveToBeActive = false;

        [Space]
        [SerializeField] Transform workPosition;

        public Action onEndWork;
        public bool isWorking = false;
        public float WorkTime { get => workTime; set => workTime = value; }

        protected LoadSystem loadSystem;
        protected Character.Character workingCharacter;

        protected GameObject character;

        protected void Awake()
        {
            if (workConfig == null) return;
            workConfig.OnConfigChanged += UpdateObject;
        }

        protected virtual void Start()
        {
            workStatus.OnLoadEnd += AfterWork;
            loadSystem = new(workTime, 0.2f);

            UpdateObject();
        }

        public void SetCharacter(GameObject character) => this.character = character;

        protected virtual void UpdateObject()
        {
            workTime = workConfig.objectData.workTime;
            UpdateWorkTime();

            if (workConfig.objectData.isActive || haveToBeActive) Open();
            else
            {
                gameObject.SetActive(false);
            }

            character.gameObject.SetActive(workConfig.objectData.isHaveWorker);
        }
        protected void UpdateWorkTime() => loadSystem.UpdateLoadTime(workTime);

        protected virtual void Open()
        {
            if (gameObject.activeInHierarchy) return;
            gameObject.SetActive(true);
            workConfig.objectData.isActive = true;
        }

        public virtual void AfterWork()
        {
            EndWork();
        }

        private void OnDestroy()
        {
            workStatus.OnLoadEnd -= AfterWork;

            if (workConfig == null) return;
            workConfig.OnConfigChanged -= UpdateObject;
        }

        public virtual void Work(bool isWork, Character.Character character)
        {
            if (isWork && workingCharacter == null)
            {
                isWorking = true;
                this.workingCharacter = character;
                workStatus.StartLoad(loadSystem);
            }
            else
            {
                isWorking = false;
                this.workingCharacter = null;
                workStatus.EndLoad();
            }
        }

        public Vector3 GetWorkPosition() => workPosition.position;

        private void EndWork()
        {
            workingCharacter.EndWork();
            this.workingCharacter = null;
            onEndWork?.Invoke();
        }
    }
}