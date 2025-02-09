using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    public class StationItemUpgrade : ItemUpgrade
    {
        [SerializeField] List<GameObject> toShowAfterBuy;
        [SerializeField] List<GameObject> toHideAfterBuy;

        public override void Buy()
        {
            if (ShopManager.instance.Buy(price))
            {
                upgrade.Buy();
                AfterBuy();
            }
        }

        private void AfterBuy()
        {
            for (int i = 0; i < toShowAfterBuy.Count; i++) {
                toShowAfterBuy[i].SetActive(true);
            }

            for (int i = 0; i < toHideAfterBuy.Count; i++)
            {
                toHideAfterBuy[i].SetActive(false);
            }
        }
    }
}