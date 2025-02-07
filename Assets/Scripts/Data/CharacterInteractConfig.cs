using System.IO;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "CharacterInteractConfig", menuName = "Character/InteractConfig")] 
    
    public class CharacterInteractConfig : ScriptableObject
    {
        public LayerMask interactLayerMask;
        public float interactRadius;
        public Vector3 interactOffset;
    }
} 