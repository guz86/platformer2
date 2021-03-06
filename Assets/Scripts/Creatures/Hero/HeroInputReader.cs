using UnityEngine;
using UnityEngine.InputSystem;

namespace Creatures.Hero
{
    public class HeroInputReader : MonoBehaviour
    {
        // пользовательский ввод
        [SerializeField] private Hero _hero;

        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }
    
        // нажали на E, и вызвали метод из героя
        public void OnInteract(InputAction.CallbackContext context)
        {
            //if (context.canceled)
            // когда действие совершилось performed
            if (context.performed)
            {
                _hero.Interact();
            }
       
        }    
        // нажали на ЛКМ, и вызвали атаку из героя
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Attack();
            }
       
        }
    
        // кидаем меч на ПКМ
        // на герое в PlayerInput в Events на герое в Trow добавляем вызов OnTrow
        public void OnThrow(InputAction.CallbackContext context)
        {
            //if (context.performed)
            if (context.started)
            {
                // мы должны зажать кнопку и когда опустим произвести бросок
                _hero.StartThrowing();
                //_hero.Throw();
            }
           if (context.canceled)
            {
                // отпустили кнопку
                _hero.PerformThrowing();
            }
        }
        
        // нажали на Q, выпили зелье из инвентаря
        public void OnUsePotion(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.UsePotion();
            }
       
        }
    }
}
 