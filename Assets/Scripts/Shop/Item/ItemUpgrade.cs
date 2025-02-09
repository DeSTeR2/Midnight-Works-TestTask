using Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ItemUpgrade : MonoBehaviour
    {
        public new string name;
        public string description;
        public int price;
        public string boughtText;
        public Sprite objectSprite;
        [SerializeField] BuyType type;
        
        ScriptableObject upgradeObject;

        public IBuy upgrade;
        IGetUpgradeObject getUpgradeObject;

        public Action OnBuy;

        public void SetUpgradeObject(ScriptableObject upgradeObject)
        {
            this.upgradeObject = upgradeObject;

            if (upgradeObject is IGetUpgradeObject)
            {
                getUpgradeObject = (IGetUpgradeObject)upgradeObject;
                upgrade = BuyStraregyFactory.GetStraregy(type, getUpgradeObject);
            } else
            {
                Debug.LogError($"{upgradeObject.name} not implements IGetUpgradeObject");
            }
        }

        public virtual void Buy()
        {
            if (upgrade.IsFull() == false && ShopManager.instance.Buy(price))
            {
                upgrade.Buy();
                OnBuy?.Invoke();
            }
        }
    }

    public enum BuyType
    {
        Worker,
        Time,
        Station,
        PutCapasity,
        TakeCapasity,
        StorageCapasity,
        DeliveryMan
    }
}