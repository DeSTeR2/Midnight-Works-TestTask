using Resources;
using UnityEngine;

namespace InteractObjects.Work.Actions
{
    public class Action
    {
        public Vector3 position;
        public ActionType actionType;
        public ResourceType resourceType;

        public Action(Vector3 position, ActionType actionType)
        {
            this.position = position;
            this.actionType = actionType;

            resourceType = ResourceType.None;
        }

        public Action(Vector3 position, ActionType actionType, ResourceType resourceType)
        {
            this.position = position;
            this.actionType = actionType;
            this.resourceType = resourceType;
        }
    }
}