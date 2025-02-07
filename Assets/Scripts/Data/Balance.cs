using System;
using UnityEngine;
using Utils;

namespace Data
{
    [CreateAssetMenu(fileName = "Balance", menuName = "Balance/Balance")]
    public class Balance : ScriptableObject, IFile
    {
        public string FileName => "Balance.json";

        public BalanceData balanceData = new();

        public static Action<int> OnBalanceChange;

        public int BalanceValue
        {
            get => balanceData.balance;
            set
            {
                balanceData.balance = value;
                OnBalanceChange?.Invoke(balanceData.balance);
            }
        }

        public void Assign<T>(T data) where T : IData
        {
            balanceData.Copy(data);
        }

        public bool Load()
        {
            Balance balance = this;
            return FileSystem.Load<BalanceData, Balance>(FileName, ref balance);
        }

        public void Save()
        {
            FileSystem.Save(FileName, balanceData);
        }
    }

    [Serializable]
    public class BalanceData : IData 
    {
        public int balance;

        public BalanceData() {
            balance = 0;
        }

        public void Copy(IData data)
        {
            if (data != null)
            {
                balance = ((BalanceData)data).balance;
            }
        }
    }
}