using System;
using UnityEngine;

namespace Model.Definitions
{
    // CreateAssetMenu для создания наших объектов в директории проекта через Create
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]
    // описания предметов инвентаря
    
    // ScriptableObject специальный объект который
    // позволяет создавать инстансы для его наследников как Ассет
    
    public class InventoryItemsDef : ScriptableObject
    {
        // для структуры массив, т.к. незименная
        [SerializeField] private ItemDef[] _items;

        public ItemDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id)
                {
                    return itemDef;
                }
            }
            //т.к. структура
            return default; 
    }
        
        //откроем список для редактора для доступа из InventoryIdAttributeDrawer
#if UNITY_EDITOR
        public ItemDef[] itemsForEditor => _items;
#endif
        
        //т.к. описание изменяться не должно делаем структуру
        [Serializable]
        public struct ItemDef
        {
            [SerializeField] private string _id;
            public string Id => _id;

            public bool IsVoid => string.IsNullOrEmpty(_id);
        }
    }
}