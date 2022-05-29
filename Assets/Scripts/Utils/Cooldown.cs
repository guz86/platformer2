using System;
using UnityEngine;

namespace Utils
{
    // для настройки из редактора
    [Serializable]
    public class Cooldown
    {
        // добавим на героя время кулдауна _value
        [SerializeField] private float _value;

        private float _timesUp;
        
        public void Reset()
        {
            // получаем будущее время, когда закончится КД
            _timesUp = Time.time + _value;
        }

        // если время которое выставлено, прошло, значит готов к броску
        public bool IsReady => _timesUp <= Time.time;
    }
}