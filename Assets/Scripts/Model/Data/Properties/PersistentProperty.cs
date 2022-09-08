using System;
using UnityEngine;

namespace Model.Data.Properties
{
    // реализация для сохранения любого типа свойств
    // базовый класс
    [Serializable]
    public abstract class PersistentProperty<TPropertyType>
    {
        // поле для отображения в редакторе
        [SerializeField] private TPropertyType _value;
        
        // то что хранится на диске
        private TPropertyType _stored;
        
        // на случай если нет какого-то значения
        private TPropertyType _defaultValue;
        
        // для отлавливания изменений в нашем свойстве
        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);

        public event OnPropertyChanged OnChanged;
        
        //для утилизации _defaultValue, передадим его через конструктор
        public PersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        } 
        
        
        // для того чтобы забирать значение
        public TPropertyType Value
        {
            get => _stored;
            set
            {
                // проверяем значение прежде чем записывать, т.к. Write жрет ресурсов
                var Equals = _stored.Equals(value);
                if (Equals) return;

                var oldValue = _stored;
                
                Write(value);
                _stored = _value = value;
                
                OnChanged?.Invoke(value, oldValue);
            }
        }

        // прочитаем значение из постоянного хранилища
        protected void Init()
        {
            _stored = _value = Read(_defaultValue);
        }
        
        // методы которые реализуем в наследниках для записи и чтения
        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);
        
        
        // для сохранения данных при передвижении ползунка в редакторе, подключается из GameSettings
        public void Validate()
        {
            if (!_stored.Equals(_value)) 
                Value = _value;
        }
        
        
    }
}