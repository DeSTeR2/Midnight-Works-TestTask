using UnityEngine;

namespace CustomSystems
{
    public class InitSystem : MonoBehaviour
    {
        private void Awake()
        {
            SellSystem.InitSystem();
            RequesSystem.InitSystem();
        }

        private void OnDestroy()
        {
            RequesSystem.Delete();
            SellSystem.Delete();
        }
    }
}