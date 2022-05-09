using UnityEngine;

// у объекта с которым столнулись проверяет healthComponent и вызывает с него метод Apply
public class ModifyHealthComponent : MonoBehaviour
{
    [SerializeField] private int _hpDelta;

    //публичный метод который нанесет damage
    public void Apply(GameObject target)
    {
        // если на объекте есть компонент
        var healthComponent = target.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.ModifyHealth(_hpDelta); // модифицируем
        }
    }
}