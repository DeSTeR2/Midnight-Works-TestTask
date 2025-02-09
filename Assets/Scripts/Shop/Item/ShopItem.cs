using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopItem : MonoBehaviour
    {
        [Header("Small object")]
        [SerializeField] Image smallItemImage;
        [SerializeField] TextMeshProUGUI itemSmallNameText;
        [SerializeField] TextMeshProUGUI itemSmallDescriptionText;
        [SerializeField] Button openBigObject;

        [Header("ItemUpgrades")]
        [SerializeField] Transform upgradeParent;

        [Header("Info")]
        [SerializeField] new string name;
        [SerializeField, TextArea] string shortDescription;
        [SerializeField] Sprite objectSprite;
        [SerializeField] ScriptableObject dataInScriptable;

        public static Action<ShopItem> OnOpenBig;

        List<ItemUpgrade> upgrades = new();

        protected virtual void Start()
        {
            AssignAllInfo();
            openBigObject.onClick.AddListener(OpenBig);

            ItemUpgrade[] upgrades = upgradeParent.GetComponents<ItemUpgrade>();
            for (int i = 0; i < upgrades.Length; i++)
            {
                ItemUpgrade upgrade = upgrades[i];
                upgrade.SetUpgradeObject(dataInScriptable);
                this.upgrades.Add(upgrade);
            }
        }

        private void AssignAllInfo()
        {
            smallItemImage.sprite = objectSprite;
            itemSmallNameText.text = name;
            itemSmallDescriptionText.text = shortDescription;
        }

        private void OpenBig()
        {
            OnOpenBig?.Invoke(this);    
        }

        public virtual List<ItemUpgrade> GetItemUpgrades() => upgrades;
    }
}   