using InteractObjects.Place;
using Resources;
using UnityEngine;
using UnityEngine.AI;

namespace InteractObjects
{
    public class ObjectPlace : MonoBehaviour, IObjectPlace
    {
        [SerializeField] Transform placePositionsParent;
        [SerializeField] Transform placeBoundariesParent;
        [SerializeField] bool isOnFloor;

        [Space]
        [SerializeField] int capability;

        protected PlaceObjects placeStrategy;

        public bool IsOnFloor => isOnFloor;

        protected virtual void Start()
        {
            InitalSetup();
        }

        protected void InitalSetup()
        { 
            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            float horizontalSize = 0;
            float verticalSize = 0;

            float minX = float.PositiveInfinity, maxX = float.NegativeInfinity;
            float minZ = float.PositiveInfinity, maxZ = float.NegativeInfinity;
            for (int i = 0; i < placeBoundariesParent.childCount; i++)
            {
                Vector3 pos = placeBoundariesParent.GetChild(i).transform.localPosition;
                lineRenderer.SetPosition(i, pos);

                minX = Mathf.Min(minX, pos.x);
                maxX = Mathf.Max(maxX, pos.x);

                minZ = Mathf.Min(minZ, pos.z);
                maxZ = Mathf.Max(maxZ, pos.z);
            }

            horizontalSize = maxX - minX;
            verticalSize = maxZ - minZ;

            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(horizontalSize, 2, verticalSize);
        }

        public virtual void PutObject(GameObject go)
        {
            placeStrategy.Place(go);
        }

        public virtual bool CanPlace(ResourceType type) => placeStrategy.CanPlace();
        public int ObjectNumber() => placeStrategy.ObjectNumber();
        public int RemaintObjectsToFull() => placeStrategy.MaxObjects() - ObjectNumber();
        public void SetCapability(int capability)
        {
            this.capability = capability;

            if (placeStrategy == null) {
                placeStrategy = new(placePositionsParent);
            }

            placeStrategy.SetCapability(capability);
        }
    }
}   