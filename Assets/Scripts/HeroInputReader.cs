using UnityEngine;
using UnityEngine.InputSystem;

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
        if (context.performed)
        {
            _hero.Throw();
        }
        
    }
}
 