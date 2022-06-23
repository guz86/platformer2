using Model.Definitions;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Interactions
{
    // висит на двери и требует наличия ключа
    public class RequireItemComponent : MonoBehaviour
    {
        [InventoryId][SerializeField] private string _id;
        [SerializeField] private int _count;
        [SerializeField] private bool _removeAfterUse;
        
        // в _onSuccess будем отключать коллайдер и вызывать анимацию открывания двери
        [SerializeField] private UnityEvent _onSuccess;
        [SerializeField] private UnityEvent _onFail;

        // будем вызывать Check() в InteractableComponent при взаимодействии героя
        public void Check()
        {
            // напрямую обращаемся к инвентарю
            var session = FindObjectOfType<GameSession>();
            // посчитали количество необходимых предметов
            var numItems = session.Data.Inventory.Count(_id);
            if (numItems >= _count)
            {
                // если хватает предметов, удаляем из инвернтаря и возвращаем
                if (_removeAfterUse)
                {
                   session.Data.Inventory.Remove(_id,_count); 
                   _onSuccess?.Invoke();
                }
                
            }
            else
            {
                _onFail?.Invoke();
            }
        }
    }
}