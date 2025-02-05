using System;
using UnityEngine;

namespace InteractObjects.Work
{
    [CreateAssetMenu(fileName = "CraftingReceipt", menuName = "Resources/Crafting")]
    public class CraftingReceipt : ScriptableObject
    {
        public CraftSetting craftFrom;
        public CraftSetting craftResult;
    }

    [Serializable]
    public struct CraftSetting
    {
        public InteractObject craftItem;
        public int count;
    }
}