using System;
using System.Collections.Generic;
using Model.Definitions;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class InventoryData
    {
        // список всех наших предметов
        // сам предмет InventoryItemData
        [SerializeField] private List<InventoryItemData> _inventory = 
            new List<InventoryItemData>();
        
        //для того чтобы реагировать на поднятие предметов
        public delegate void OnInventoryChanged(string id, int value);

        // ивент переменную с типом делегата, подпишем на нее на старте class Hero подписчика
        public OnInventoryChanged OnChanged;
        
        // помещать и удалять предметы из инвентаря
        public void Add(string id, int value)
        {
            if (value <= 0) return;

            // проверка инвентаря 
            var itemDef = DefsFacade.I.Items.Get(id);
            // если предмета не существует, выходим
            if (itemDef.IsVoid) return;
            
            
            var item = GetItem(id);
            
            /*// если предмет лежит в инвентаре то добавляем к текущему значению value
            if (item != null)
            {
                item.Value += value;
            }
            // иначе создаем предмет в инвентаре
            else
            {
                item = new InventoryItemData(id, value);
                // добавим в инвентарь
                _inventory.Add(item);
            }
           */   
            
            // если нет, то добавим
            if (item == null)
            {
                item = new InventoryItemData(id);
                // добавим в инвентарь
                _inventory.Add(item);
            }
            
            // в любом случае добавим количество предметов 
                item.Value += value;
                
            // вызов подписки на событие
            OnChanged?.Invoke(id,Count(id));
        }
        
        // удаление предметов из инвентаря
        public void Remove(string id, int value)
        {
            // проверка инвентаря 
            var itemDef = DefsFacade.I.Items.Get(id);
            // если предмета не существует, выходим
            if (itemDef.IsVoid) return;
            
            
            var item = GetItem(id);
            if (item == null) return;
            
            // если не пустой, отнимаем количество у item
            item.Value -= value;

            if (item.Value <= 0) 
                _inventory.Remove(item);
            
            // вызов подписки на событие
            OnChanged?.Invoke(id,Count(id));
        }
        
        // есть ли предмет уже в инвентаре
        private InventoryItemData GetItem(string id)
        {
            // возвращаем предмет если есть в инвентаре такой же
            foreach (var itemData in _inventory)
            {
                if (itemData.Id == id)
                {
                    return itemData;
                }
            }

            return null;
        }

        // возвращаем количество предмета в инвентаре
        public int Count(string id)
        {
            var count = 0;
            foreach (var item in _inventory)
            {
                if (item.Id == id)
                {
                    count += item.Value;
                }
            }

            return count;
        }
    }
    
    [Serializable]
    public class InventoryItemData
    {
        [InventoryId] public string Id;
        public int Value;
        
        // конструктор для item = new InventoryItemData(id, value);
        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}