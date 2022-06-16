using System;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;
        
        //public int Coins;
        public int Hp;
        //public bool IsArmed;
        
        // открываем для доступа из Hero
        public InventoryData Inventory => _inventory; 

        // так как будут добавлять новые параметры, не всегда удобно
        // реализовывать таким образом функцию, придется каждый раз дублировать
        public PlayerData Clone()
        {
            // return new PlayerData()
            // {
            //     Coins = Coins,
            //     Hp = Hp,
            //     IsArmed = IsArmed
            // };
        
            // загрузка другим образом через json
            // загоняем текущий клас в json - в строковый тип
            var json = JsonUtility.ToJson(this);
            // десерриализовать через FromJson обратно, читаем json и создаем
            // его в новом классе, не важно сколько данных
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
}
