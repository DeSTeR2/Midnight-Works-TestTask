using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TutorialPanel : MonoBehaviour
    {
        [SerializeField] Transform pageTransform;
        [SerializeField] TextMeshProUGUI pageNumberText;
        [SerializeField] Button nextBtn;
        [SerializeField] Button prevBtn;
        [SerializeField] Button closeBtn;

        int index = -1;
        List<GameObject> pages = new(); 

        public void Start()
        {
            closeBtn.onClick.AddListener(Close);
            nextBtn.onClick.AddListener(delegate {
                ChangePage(1);
            });

            prevBtn.onClick.AddListener(delegate {
                ChangePage(-1);
            });

            for (int i = 0; i < pageTransform.childCount; i++) { 
                pages.Add(pageTransform.GetChild(i).gameObject);
            }

            ChangePage(1);
        }

        private void ChangePage(int delta)
        {
            index += delta;
            if (index == 0)
            {
                prevBtn.gameObject.SetActive(false);
                nextBtn.gameObject.SetActive(true);
                closeBtn.gameObject.SetActive(false);
            }
            else if (index == pages.Count - 1) {
                prevBtn.gameObject.SetActive(true);
                nextBtn.gameObject.SetActive(false);
                closeBtn.gameObject.SetActive(true);
            } 
            else
            {
                prevBtn.gameObject.SetActive(true);
                nextBtn.gameObject.SetActive(true);
                closeBtn.gameObject.SetActive(false);
            }

            for (int i = 0; i < pages.Count; i++) { 
                pages[i].SetActive(false);
            }
            pages[index].SetActive(true);

            pageNumberText.text = $"Page {index + 1} of {pages.Count}";
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
