using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    public class BigObject : MonoBehaviour
    {
        [SerializeField] List<BigObjectDataAssigner> dataAssigners;

        public void Show(List<ItemUpgrade> data)
        {
            gameObject.SetActive(true);
            for (int i = 0; i < data.Count; i++)
            {
                dataAssigners[i].Assign(data[i]);
            }

            for (int i = data.Count; i < dataAssigners.Count; i++)
            {
                dataAssigners[i].Hide();
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}