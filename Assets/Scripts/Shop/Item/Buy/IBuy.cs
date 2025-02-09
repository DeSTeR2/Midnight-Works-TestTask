namespace Shop
{
    public interface IBuy
    {
        void Buy();
        object GetUpgradeValue();
        bool IsFull();
    }
}