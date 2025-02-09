namespace Data
{
    public interface IBuyWorker
    {
        int BuyWorker(bool watch = false);
        bool IsUpradeTimeFull();
    }

    public interface IBaseDataUpgrade : IBuyWorker
    {
        bool BuyStation(bool watch = false);
        int UpgradeTime(bool watch = false);
    }

    public interface IWorkObjectUpgrade : IBaseDataUpgrade
    {
        int UpgradeTakeCapasity(bool watch = false);
        bool IsUpgradeTakeCapasityFull();
        int UpgradePutCapasity(bool watch = false);
        bool IsUpgradePutCapasityFull();
    }

    public interface IGetUpgradeObject
    {
        object GetUpgradeObject();
    }
}