using Data;

namespace Shop
{
    public class TimeBuy : IBuy
    {
        IBaseDataUpgrade upgrade;
        public TimeBuy(object workObjectUpgrade)
        {
            upgrade = (IBaseDataUpgrade)workObjectUpgrade;
        }

        public void Buy()
        {
            upgrade.UpgradeTime();
        }

        public object GetUpgradeValue()
        {
            return upgrade.UpgradeTime(true);
        }

        public bool IsFull()
        {
            return upgrade.IsUpradeTimeFull();
        }
    }
}