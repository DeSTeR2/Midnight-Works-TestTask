using UnityEngine;

namespace CustomSystems
{
    public class InitSystem : MonoBehaviour
    {
        float timer = 0;

        private void Start()
        {
            RequesSystem.InitSystem();
        }

        private void OnDestroy()
        {
            RequesSystem.Delete();
        }

/*        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                timer = 0;
                DebugRequests();
            }
        }*/

        private void DebugRequests()
        {
            Debug.Log(RequesSystem.Requests());
        }
    }
}