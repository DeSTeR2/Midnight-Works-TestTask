using Resources;

namespace InteractObjects.Place
{
    public class StorageObject : ObjectPlace
    {
        public InteractObject GetResourse(ResourceType type)
        {
            try
            {
                return placeStrategy.FindRightResource(type);
            }
            catch {
                throw new System.Exception("There is no resource");
            }
        }
    }
}