using InteractObjects.Place;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace InteractObjects
{
    public class ObjectPlace : MonoBehaviour, IObjectPlace
    {
        [SerializeField] Transform placePositionsParent;
        [SerializeField] Transform placeBoundariesParent;
        [SerializeField] bool isOnFloor;

        PlaceObjects placeStrategy;

        public bool IsOnFloor => isOnFloor;

        private void Start()
        {
            placeStrategy = new(placePositionsParent);

            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            float horizontalSize = 0;
            float verticalSize = 0;

            float minX = float.PositiveInfinity, maxX = float.NegativeInfinity;
            float minZ = float.PositiveInfinity, maxZ = float.NegativeInfinity;
            for (int i = 0; i < placeBoundariesParent.childCount; i++) {
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

        public void PutObject(GameObject go)
        {
            placeStrategy.Place(go);
        }
    }
}