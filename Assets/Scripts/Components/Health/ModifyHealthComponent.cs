using UnityEngine;

// у объекта с которым столнулись проверяет healthComponent и вызывает с него метод Apply
namespace Components.Health
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _hpDelta;

        //публичный метод который изменит здоровье
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
}