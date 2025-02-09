using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SceneLoading : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI loadingText;

        public void LoadScene(int sceneIndex)
        {
            gameObject.SetActive(true);
            StartCoroutine(AsyncLoader(sceneIndex));
        }

        int index = 0;

        IEnumerator AsyncLoader(int sceneIndex) {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            index = 0;

            while (!operation.isDone) {
                loadingText.text = "Loading";
                for (int i = 0; i < index; i++) {
                    loadingText.text += ".";
                }

                index++;
                if (index == 4) index = 0;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}