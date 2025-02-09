using System;
using UnityEngine;

namespace CustomSystems
{
    public class GlobalTicker : MonoBehaviour
    {
        public static Action OnTick;

        private void FixedUpdate()
        {
            OnTick?.Invoke();
        }
    }
}