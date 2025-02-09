using Data;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ShowBalance : MonoBehaviour {
        [SerializeField] Balance balance;
        [SerializeField] TextMeshProUGUI text;
        private void Start()
        {
            Balance.OnBalanceChange += UpdateBalance;
            UpdateBalance(balance.BalanceValue);
        }

        private void UpdateBalance(int balance)
        {
            text.text = balance.ToString();
        }

        private void OnDestroy()
        {
            Balance.OnBalanceChange -= UpdateBalance;
        }
    }
}