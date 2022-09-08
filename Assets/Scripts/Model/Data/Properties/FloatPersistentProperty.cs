using System;
using UnityEngine;

namespace Model.Data.Properties
{
    // для отображения полей в ассете  [Serializable]
    [Serializable]
    // конкретная реализация - запись данных в PlayerPrefs
    public class FloatPersistentProperty: PrefsPersistentProperty<float>
    {
        
        public FloatPersistentProperty(float defaultValue, string key) : base(defaultValue, key)
        {
            // изначально проинициализируем
            Init();
        }

        protected override void Write(float value)
        {
            PlayerPrefs.SetFloat(Key, value);
            PlayerPrefs.Save();
        }

        protected override float Read(float defaultValue)
        {
            return PlayerPrefs.GetFloat(Key, defaultValue);
        }

    }
}