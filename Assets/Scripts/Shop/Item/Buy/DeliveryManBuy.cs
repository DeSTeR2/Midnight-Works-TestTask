using Data;

namespace Shop
{
    public class DeliveryManBuy : IBuy
    {
        IBuyWorker upgrade;
        public DeliveryManBuy(object workObjectUpgrade)
        {
            upgrade = (IBuyWorker)workObjectUpgrade;
        }

        public bool IsFull()
        {
            return upgrade.IsUpradeTimeFull();
        }

        public void Buy()
        {
            upgrade.BuyWorker();
        }

        public object GetUpgradeValue()
        {
            return upgrade.BuyWorker(true);
        }
    }
}