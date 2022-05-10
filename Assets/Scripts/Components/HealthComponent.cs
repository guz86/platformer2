using UnityEngine;
using UnityEngine.Events;

// на герое, _onDamage будет передача в TakeDamage() метод, 
// _onDie   будет передача в  ReloadLevelComponent Reload
public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onHeal; // исцеление
    [SerializeField] private UnityEvent _onDie;

    // вызывается из DamageComponent, в 1 случае списывает жизни, в 2 перезагрузка
    public void ModifyHealth(int healthDelta)
    {
        _health += healthDelta;
        // урон - аниматор, вылетающие монеты, сила вверх
        if (healthDelta < 0)
        {
            _onDamage?.Invoke();
        }
        // лечение
        if (healthDelta > 0)
        {
            _onHeal?.Invoke();
        }
        // смерть - перезагрузка уровня
        if (_health <= 0)
        {
            _onDie?.Invoke();
        }
    }
}