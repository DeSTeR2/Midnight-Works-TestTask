using RequestManagment;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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

        /*private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                timer = 0;
                DebugRequests();
            }
        }*/

        private Rect displayRect = new Rect(10, 10, 400, 300);

        private void OnGUI()
        {
            // Retrieve the long text.
            string longText = RequesSystem.Requests();

            // Create a style for the label with word wrapping and white text.
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                wordWrap = true,
                normal = { textColor = Color.black }
            };

            // Create a style for the box with a black background.
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = Texture2D.blackTexture;

            // Draw the black background box.
            GUI.Box(displayRect, GUIContent.none, boxStyle);

            // Optionally, define some padding inside the box.
            Rect textRect = new Rect(displayRect.x + 10, displayRect.y + 10,
                                       displayRect.width - 20, displayRect.height - 20);

            // Draw the text within the padded area.
            GUI.Label(textRect, longText, labelStyle);
        }


        private void DebugRequests()
        {
            Debug.Log(RequesSystem.Requests());
        }
    }
}