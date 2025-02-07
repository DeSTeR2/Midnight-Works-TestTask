using Character;
using CustomSystems;
using Resources;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InteractObjects.Work
{
    public class WorkObject : MonoBehaviour, IWorkPlace
    {
        [SerializeField] protected float workTime;
        [SerializeField] protected WorkStatus workStatus;

        [Space]
        [SerializeField] Transform workPosition;

        public Action onEndWork;
        public bool isWorking = false;
        public float WorkTime => workTime;

        protected LoadSystem loadSystem;
        protected Character.Character workingCharacter;

        protected virtual void Start()
        {
            workStatus.OnLoadEnd += AfterWork;
            loadSystem = new(workTime, 0.2f);
        }

        public virtual void AfterWork()
        {
            EndWork();
        }

        private void OnDestroy()
        {
            workStatus.OnLoadEnd -= AfterWork;
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