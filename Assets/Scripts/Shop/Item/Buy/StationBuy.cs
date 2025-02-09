using Data;

namespace Shop
{
    public class StationBuy : IBuy
    {
        IBaseDataUpgrade upgrade;
        public StationBuy(object workObjectUpgrade)
        {
            upgrade = (IBaseDataUpgrade)workObjectUpgrade;
        }

        public void Buy()
        {
            upgrade.BuyStation();
        }

        public object GetUpgradeValue()
        {
            return upgrade.BuyStation(true);
        }

        public bool IsFull()
        {
            return upgrade.BuyStation(true);
        }
    }
}