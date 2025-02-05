using CustomSystems;
using System;
using Unity.Loading;
using UnityEngine;
using UnityEngine.UI;

namespace InteractObjects.Work
{
    public class WorkStatus : MonoBehaviour
    {
        [SerializeField] Image loadImage;
        LoadSystem loadSystem;

        public Action OnLoadEnd;

        public void StartLoad(LoadSystem loadSystem)
        {
            gameObject.SetActive(true);
            this.loadSystem = loadSystem;
            
            loadSystem.Load();
            loadSystem.OnTickEnd += UpdateStatus;

            loadImage.fillAmount = 0;
        }

        public void EndLoad()
        {
            gameObject.SetActive(false);
            loadSystem.OnTickEnd -= UpdateStatus;
        }

        private void UpdateStatus(float state)
        {
            loadImage.fillAmount = state;

            if (state >= 1)
            {
                Debug.Log("State is 1");
                loadImage.fillAmount = 1;
                loadSystem.OnTickEnd -= UpdateStatus;
                OnLoadEnd?.Invoke();
            }
        }
    }
}