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
        private Action onEndWork;

        public float WorkTime => workTime;
        public Action OnEndWork => onEndWork;

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
                Debug.Log("Start work");
                this.workingCharacter = character;
                workStatus.StartLoad(loadSystem);
            }
            else
            {
                Debug.Log("End work in Work");
                this.workingCharacter = null;
                workStatus.EndLoad();
            }
        }

        private void EndWork()
        {
            Debug.Log("End Work");
            workingCharacter.EndWork();
            this.workingCharacter = null;
            OnEndWork?.Invoke();
        }
    }
}