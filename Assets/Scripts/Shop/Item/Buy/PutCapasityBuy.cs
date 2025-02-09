using Data;

namespace Shop
{
    public class PutCapasityBuy : IBuy
    {
        IWorkObjectUpgrade upgrade;
        public PutCapasityBuy(object workObjectUpgrade)
        {
            upgrade = (IWorkObjectUpgrade)workObjectUpgrade;
        }

        public bool IsFull()
        {
            return upgrade.IsUpgradePutCapasityFull();
        }

        public void Buy()
        {
            upgrade.UpgradePutCapasity();
        }

        public object GetUpgradeValue()
        {
            return upgrade.UpgradePutCapasity(true);
        }
    }
}