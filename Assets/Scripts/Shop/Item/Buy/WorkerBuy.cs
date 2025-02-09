using Data;

namespace Shop
{
    public class WorkerBuy : IBuy
    {
        IBaseDataUpgrade upgrade;
        public WorkerBuy(object workObjectUpgrade)
        {
            upgrade = (IBaseDataUpgrade)workObjectUpgrade;
        }

        public void Buy()
        {
            upgrade.BuyWorker();
        }

        public object GetUpgradeValue()
        {
            return upgrade.BuyWorker(true);
        }

        public bool IsFull()
        {
            return (upgrade.BuyWorker(true) == 1 ? true : false);
        }
    }
}