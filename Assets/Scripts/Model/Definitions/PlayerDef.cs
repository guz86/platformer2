using UnityEngine;

namespace Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        // здесь будем хранить настройки для Player
        [SerializeField] private int _inventorySize;
        
        public int InventorySize => _inventorySize;
    }
}