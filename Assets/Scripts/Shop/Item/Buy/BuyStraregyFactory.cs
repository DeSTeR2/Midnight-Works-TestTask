using Data;
using UnityEditor;

namespace Shop
{
    public static class BuyStraregyFactory
    {
        public static IBuy GetStraregy(BuyType type, IGetUpgradeObject upgrade)
        {
            object upradeObject = upgrade.GetUpgradeObject();

            switch (type)
            {
                case BuyType.Worker:
                    return new WorkerBuy(upradeObject);
                case BuyType.Time:
                    return new TimeBuy(upradeObject);
                case BuyType.Station:
                    return new StationBuy(upradeObject);
                case BuyType.PutCapasity:
                    return new PutCapasityBuy(upradeObject);
                case BuyType.TakeCapasity:
                    return new TakeCapasityBuy(upradeObject);
                case BuyType.StorageCapasity:
                    return new StorageCapasityBuy(upradeObject);
                case BuyType.DeliveryMan:
                    return new DeliveryManBuy(upradeObject);
            }

            return default;
        }
    }
}