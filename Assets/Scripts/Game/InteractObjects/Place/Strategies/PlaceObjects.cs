using CustomSystems;
using Resources;
using System.Collections.Generic;
using UnityEditor.SearchService;
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

        public bool CanPlace(int objectNumber = 1)
        {
            UpdateIndex();
            if (index + objectNumber <= positions.Count && index + objectNumber <= config.capability) return true;
            return false;
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

        public void RemoveObjects(int objectNumber)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i].childCount > 0)
                {
                    InteractObject go = positions[i].GetChild(0).gameObject.GetComponent<InteractObject>();
                    go.transform.parent = null;
                    ResourceSystem.instance.BackObject(go);
                    objectNumber--;
                }

                if (objectNumber == 0) return;
            }
        }
        public int ObjectNumber()
        {
            int number = 0;
            for (int i = 0; i < config.capability; i++)
            {
                number += (positions[i].childCount == 1 ? 1 : 0);
            }
            return number;
        }
        public int MaxObjects() => config.capability;
    
        public InteractObject FindRightResource(ResourceType resourceType)
        {
            for (int i=0; i < config.capability; i++)
            {
                if (positions[i].childCount == 1)
                {
                    InteractObject obj;
                    if (positions[i].GetChild(0).gameObject.TryGetComponent<InteractObject>(out obj))
                    {
                        if (obj.ResourceType == resourceType) {
                            return obj;
                        }
                    }
                }
            }
            throw new System.Exception("There is no right resource");
        }
    }
}