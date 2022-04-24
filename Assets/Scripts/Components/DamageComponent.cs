using UnityEngine;

// у объекта с которым столнулись проверяет healthComponent и вызывает с него метод ApplyDamage
public class DamageComponent : MonoBehaviour
{
    [SerializeField] private int _damage;

    //публичный метод который нанесет damage
    public void ApplyDamage(GameObject target)
    {
        var healthComponent = target.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.ApplyDamage(_damage);
        }
    }
}