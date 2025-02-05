using System.Collections.Generic;
using UnityEngine;

namespace InteractObjects.Place
{
    public class PlaceObjects
    {
        List<Transform> positions;
        int index = 0;

        ObjectPlaceConfig config;

        public PlaceObjects(Transform positions, ObjectPlaceConfig config)
        {
            this.positions = new();
            foreach (Transform t in positions)
            {
                this.positions.Add(t);
            }

            this.config = config;
        }

        public bool Place(GameObject go)
        {
            Debug.LogWarning("TODO: place with capability");
            UpdateIndex();
            if (index < positions.Count)
            {
                go.transform.parent = positions[index++];
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                return true;
            }
            else return false;
        }

        private void UpdateIndex()
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i].childCount == 0) {
                    index = i;
                    return;
                }
            }
        }
    }
}