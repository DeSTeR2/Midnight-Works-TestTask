using Character.Worker;
using CustomSystems;
using Resources;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class Customer : AIWalkable
    {
        [SerializeField] Image dialogImage;
        [SerializeField] GameObject dialogPanel;
        [SerializeField] CustomerEmotions customerEmotions;

        float satisfactionMoveDownSpeed = 0.01f;
        float satisfaction = 1;

        bool onOrder = false;

        ResourceType resourceType = ResourceType.None;

        public ResourceType Order { get => resourceType; }

        private void OnEnable()
        {
            satisfaction = 1;
            onOrder = true;

            resourceType = SellSystem.CreateSellRequest();
        }

        protected override void Start()
        {
            base.Start();
            dialogPanel.gameObject.SetActive(false);
        }

        public void OrderComplete()
        {
            onOrder = false;
            ShowEmotion();
        }

        public void OrderCancel() {
            satisfaction = 0;
            onOrder = false;
            ShowEmotion();
        }

        private void ShowEmotion()
        {
            List<Emotions> emotions = customerEmotions.emotions;
            Sprite sprite = null;
            float smalestAbs = float.MaxValue;
            for (int i=0; i< emotions.Count;i++)
            {
                float abs = Mathf.Abs(satisfaction - emotions[i].satisfaction);
                if (abs < smalestAbs)
                {
                    smalestAbs = abs;
                    sprite = emotions[i].emothionSprite;
                }
            }

            dialogPanel.SetActive(true);
            dialogImage.sprite = sprite;

            DelaySystem.DelayFunction(delegate {
                dialogPanel.SetActive(false);
            }, 1f);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (onOrder)
            {
                satisfaction -= satisfactionMoveDownSpeed * Time.fixedDeltaTime;
                satisfaction = Mathf.Max(0, satisfaction);
            }
        }
    }

    [CreateAssetMenu(fileName = "CustomerEmotions", menuName = "Character/CustomerEmotions")]
    public class CustomerEmotions : ScriptableObject 
    {
        public List<Emotions> emotions;
    }

    [Serializable]
    public struct Emotions
    {
        public Sprite emothionSprite;
        public float satisfaction;
    }
}