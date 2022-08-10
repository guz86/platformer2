using System.Collections.Generic;
using Model.Data;
using UnityEngine;

namespace Components.Collectables
{
    public class CollectorComponent : MonoBehaviour, ICanAddInInventory
    {

        // используем интерфейс для собирания предметов в бочку
        // при интеракции бочка наполняется предметами (толкнуть бочку на коины)
        [SerializeField] private List<InventoryItemData> _items =
            new List<InventoryItemData>();
        
        public void AddInInventory(string id, int value)
        {
            _items.Add(new InventoryItemData(id) {Value = value});
        }

        // при взаимодействии мы забираем из бочки сложенные в нее предметы
        public void DropInInventory()
        {
            var session = FindObjectOfType<GameSession>();
            foreach (var inventoryItemData in _items)
            {
                session.Data.Inventory.Add(inventoryItemData.Id, inventoryItemData.Value);
            }
            // очищаем бочку
            _items.Clear();
        }
    }
}