using System;
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
    //  для вызова с героя метода OnHealthChange для переноса здоровья в данные сессии
    [SerializeField] private HealthChangeEvent _OnChange;

    // вызывается из DamageComponent, в 1 случае списывает жизни, в 2 перезагрузка
    public void ModifyHealth(int healthDelta)
    {
        _health += healthDelta;
        // если мы изменили здоровье, мы вызываем метод для передачи текущего уровня через OnHealthChange
        _OnChange?.Invoke(_health);
        
        
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
    
    
    [Serializable]
    public class HealthChangeEvent: UnityEvent<int>{}


    // Получаем хп из героя на Start сцены в начале
    public void SetHealth(int health)
    {
        _health = health;
    }
    
    // для изменения хп во время выполнения вызывается через 
#if UNITY_EDITOR
    [ContextMenu("Update Health")]
    private void UpdateHealth()
    {
        _OnChange?.Invoke(_health);
    }
#endif
    
}