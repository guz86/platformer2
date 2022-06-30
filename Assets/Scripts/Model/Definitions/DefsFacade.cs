using UnityEngine;

namespace Model.Definitions
{
    // нужно получить ссылку на предметы _items
    // Singleton на ресурсах
    // объект который существует в единственном экземпляре 
    // этот объект будет хранить описания, будем использовать как статическую ссылку
    
    // атрибут для создания 
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    // в директории Resources/ создаем DefsFacade и ему передаем InventoryItems
    // это будет точка с которой мы будем обращаться к дефинишенам
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _items;
        //для сохраненных настроек
        [SerializeField] private PlayerDef _player;  
            
        public InventoryItemsDef Items => _items;
        public PlayerDef Player => _player;
        
        // нужно сделать статические поля чтобы к ним можно было обращаться
        private static DefsFacade _instance;
        // если _instance пустой, загрузить 
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            // директорию откуда грузить "DefsFacade"
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }


    }
}