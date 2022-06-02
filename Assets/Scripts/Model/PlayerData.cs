using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int Coins;
    public int Hp;
    public bool IsArmed;

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
