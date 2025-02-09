using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    [RequireComponent(typeof(Button))]
    public class ItemSection : MonoBehaviour
    {
        [SerializeField] Sprite activeSprite;
        [SerializeField] Sprite inActiveSprite;
        [SerializeField] TextMeshProUGUI sectionNameText;
        [SerializeField] GameObject controllSection;

        [Space]
        [SerializeField] bool openOnAwake = false;
        [SerializeField] string sectionName;

        public static Action OnClose;
        Button controllBtn;
        Image btnImage;

        private void Awake()
        {
            OnClose += Close;
            controllBtn = GetComponent<Button>();
            btnImage = GetComponent<Image>();

            controllBtn.onClick.AddListener(Open);
            sectionNameText.text = sectionName;

            if (openOnAwake)
            {
                Open();
            }
        }

        private void Close()
        {
            btnImage.sprite = inActiveSprite;
            controllSection.SetActive(false);
        }

        private void Open()
        {
            OnClose?.Invoke();
            btnImage.sprite = activeSprite;
            controllSection.SetActive(true);
        }

        private void OnDestroy()
        {
            OnClose += Close;
        }
    }
}