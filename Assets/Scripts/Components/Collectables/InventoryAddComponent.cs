using Creatures.Hero;
using Model.Data;
using Model.Definitions;
using UnityEngine;
using Utils;

namespace Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        //  вместо AddCoinComponent ArmHeroComponent
        // что и сколько добавляем
        [InventoryId][SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add(GameObject go)
        {
            // var hero = go.GetComponent<Hero>();
            // if (hero != null)
            // {
            //     hero.AddInInventory(_id, _count);
            // }
            
            // мы создаем зависимость от целого класса выше, при этом достаем
            //только один метод
            // реализуем через интерфейс, класс Hero
            var hero = go.GetInterface<ICanAddInInventory>();
            hero?.AddInInventory(_id, _count);
            
        }
    }
}