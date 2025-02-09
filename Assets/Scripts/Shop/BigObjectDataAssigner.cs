using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class BigObjectDataAssigner : MonoBehaviour
    {
        [SerializeField] Image itemImage;
        [SerializeField] TextMeshProUGUI descriptionText;
        [SerializeField] TextMeshProUGUI priceText;
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI countText;
        [SerializeField] Button buyBtn;

        [Space]
        [SerializeField] GameObject infoObjectParent;
        [SerializeField] GameObject boughtObjectParent;

        ItemUpgrade assignedData;

        public void Assign(ItemUpgrade itemUpgrade)
        {
            assignedData = itemUpgrade;
            gameObject.SetActive(true);
            MakeData();

            assignedData.OnBuy += MakeData;
        }

        private void MakeData()
        {
            itemImage.sprite = assignedData.objectSprite;
            descriptionText.text = assignedData.description;
            priceText.text = assignedData.price.ToString();
            nameText.text = assignedData.name.ToString();

            object value = assignedData.upgrade.GetUpgradeValue();
            if ((value is bool) == false && assignedData.boughtText != "#")
            {
                countText.gameObject.SetActive(true);
                string valueString = assignedData.boughtText;
                countText.text = valueString.Replace("{value}", value.ToString());
            }
            else
            {
                countText.gameObject.SetActive(false);
            }

            buyBtn.onClick.RemoveAllListeners();
            buyBtn.onClick.AddListener(delegate {
                assignedData.Buy();
            });

            bool isFull = assignedData.upgrade.IsFull();
            if (isFull)
            {
                infoObjectParent.SetActive(false);
                boughtObjectParent.SetActive(true);
            }
            else
            {
                infoObjectParent.SetActive(true);
                boughtObjectParent.SetActive(false);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            if (assignedData != null)
            {
                assignedData.OnBuy -= MakeData;
            }
        }
    }
}