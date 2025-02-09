using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] Balance balance;

        [Space]
        [SerializeField] BigObject bigObject;
        [SerializeField] Button closeBig;
        [SerializeField] Button closeShop;

        public static ShopManager instance;


        private void Awake()
        {
            instance = this;

            closeShop.onClick.AddListener(Close);
            closeBig.onClick.AddListener(CloseBig);
            ShopItem.OnOpenBig += OpenBig;
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OpenBig(ShopItem shopItem)
        {
            bigObject.Show(shopItem.GetItemUpgrades());
        }

        private void CloseBig()
        {
            bigObject.Hide();
        }

        public bool Buy(int price)
        {
            if (price <= balance.BalanceValue)
            {
                SoundManager.instance.PlaySound(SoundType.Buy);
                balance.BalanceValue -= price;
                return true;
            }

            return false;
        }

        private void OnDestroy()
        {
            ShopItem.OnOpenBig -= OpenBig;
        }
    }
}