using Data;

namespace Shop
{
    public class StorageCapasityBuy : IBuy
    {
        StorageData upgrade;
        public StorageCapasityBuy(object workObjectUpgrade)
        {
            upgrade = ((StorageData)workObjectUpgrade);
        }

        public void Buy()
        {
            upgrade.UpgradeCapasity();
        }

        public object GetUpgradeValue()
        {
            return upgrade.UpgradeCapasity(true);
        }

        public bool IsFull()
        {
            return upgrade.capability == upgrade.maxCapability;
        }
    }
}