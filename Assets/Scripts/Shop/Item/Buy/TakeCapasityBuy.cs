using Data;

namespace Shop
{
    public class TakeCapasityBuy : IBuy
    {
        IWorkObjectUpgrade upgrade;
        public TakeCapasityBuy(object workObjectUpgrade)
        {
            upgrade = (IWorkObjectUpgrade)workObjectUpgrade;
        }

        public void Buy()
        {
            upgrade.UpgradeTakeCapasity();
        }

        public object GetUpgradeValue()
        {
            return upgrade.UpgradeTakeCapasity(true);
        }

        public bool IsFull()
        {
            return upgrade.IsUpgradeTakeCapasityFull();
        }
    }
}