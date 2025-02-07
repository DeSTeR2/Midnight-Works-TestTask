using UnityEngine;

namespace CustomSystems
{
    public class InitSystem : MonoBehaviour
    {
        float timer = 0;

        private void Awake()
        {
            RequesSystem.InitSystem();
            SellSystem.InitSystem();
        }

        private void OnDestroy()
        {
            RequesSystem.Delete();
            SellSystem.Delete();
        }

        private void DebugRequests()
        {
            Debug.Log(RequesSystem.Requests());
        }
    }
}