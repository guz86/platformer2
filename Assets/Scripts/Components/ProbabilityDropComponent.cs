using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    // будет выдавать массив из партиклов, расчет будет исходя из количества и вероятности
    public class ProbabilityDropComponent : MonoBehaviour
    {
        // количество выпадающих элементов
        [SerializeField] private int _count;
        
        [SerializeField] private DropData[] _drop;

        [SerializeField] private DropEvent _onDropCalculate;

        [SerializeField] private bool spawnOnEnable;

        // для запуска выбрасывания монет
        private void OnEnable()
        {
            if (spawnOnEnable)
            {
                CalculateDrop();
            }
        }

        // расчет Drop
        [ContextMenu("CalculateDrop")]
        public void CalculateDrop()
        {
            // создадим массив объектов указанного размера
            var itemsToDrop = new GameObject[_count];
            
            //считаем общую вероятность - сложим все Probability
            var total = _drop.Sum(dropData => dropData.Probability);
            
            // отсортируем по возрастанию вероятности, чтобы начать с элементов с меньшей
            var sortedDrop = _drop.OrderBy(dropData => dropData.Probability);
            
            
            //пройдемся по элементам и сделаем вероятностное выпадение
            var itemCount = 0;
            while (itemCount < _count)
            {
                // в цикле получим рандомное значение
                var random = UnityEngine.Random.value * total;
                var current = 0f;
                foreach (var dropData in sortedDrop)
                {
                    current += dropData.Probability;
                    // постепенно будем увеличивать вероятность
                    
                    if (current >= random)
                    {
                        // запишем в массив наш элемент
                        itemsToDrop[itemCount] = dropData.Drop;
                        itemCount++;
                        break;
                    }
                }
            }
            // передадим весь массив элементов которые нужно выкинуть
            _onDropCalculate?.Invoke(itemsToDrop);
        }
        
        [Serializable]
        public class DropData
        {
            // ссылка на префаб для дропа и вероятность
            public GameObject Drop;
            [Range (0f, 100f)] public float Probability;
        }




        // метод для выпадающих монеток
        public void SetCount(int count)
        {
            _count = count;
        }
    }
    // вынесли на уровень выше, чтобы использовать для GoContainerComponent
    [Serializable]
    public class DropEvent : UnityEvent<GameObject[]>
    {
    };
}