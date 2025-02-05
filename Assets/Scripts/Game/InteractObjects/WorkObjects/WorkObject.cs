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
        [SerializeField] float workTime;
        [SerializeField] WorkStatus workStatus;
        [SerializeField] protected ResourceType resourceType;
        private Action onEndWork;

        public float WorkTime => workTime;
        public Action OnEndWork => onEndWork;

        LoadSystem loadSystem;

        Character.Character workingCharacter;

        private void Start()
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

        public void Work(bool isWork, Character.Character character)
        {
            if (isWork && workingCharacter == null)
            {
                this.workingCharacter = character;
                workStatus.StartLoad(loadSystem);
            }
            else
            {
                this.workingCharacter = null;
                workStatus.EndLoad();
            }
        }

        private void EndWork()
        {
            workingCharacter.EndWork();
            OnEndWork?.Invoke();
        }
    }
}