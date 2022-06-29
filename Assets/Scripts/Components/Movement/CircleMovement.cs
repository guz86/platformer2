using UnityEngine;

namespace Components.Movement
{
    public class CircleMovement : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _speed = 2f;
        private Rigidbody2D[] _bodies;
        private float _time;
        private Vector2[] _positions;

        private void Awake()
        {
            UpdateContent();
        }

        private void UpdateContent()
        {
            // собираем все Rigidbody2D для объектов внутри
            _bodies = GetComponentsInChildren<Rigidbody2D>();
            // создадим массив с позициями
            _positions = new Vector2[_bodies.Length];
        }

        private void Update()
        {
            // вызовем расчет позиций
            CalculatePositions();
            //проверка для обьекта
            var isAllDead = true;
            
            for (var i = 0; i < _bodies.Length; i++)
            {
                // если есть тело, то двигаем
                if (_bodies[i])
                {
                    _bodies[i].MovePosition(_positions[i]);
                    // сбросим флаг, если есть хотя бы 1 объект
                    isAllDead = false;
                }
                
            }

            if (isAllDead)
            {
                // выключим и удалим
                enabled = false;
                Destroy(gameObject, 1f);
            }
            
            // для перемещения
            _time += Time.deltaTime;
        }

        private void CalculatePositions()
        {
            // Mathf.sin - принимает угол в радианах
            // чтобы аккуратно расставит элементы по кругу,
            // мы берем длину круга 2Пи и делим на количество элементов
            var step = 2 * Mathf.PI / _bodies.Length;
            
            // возмем позицию текущего элемента
            Vector2 containerPosition = transform.position;
            
            
            for (var i = 0; i < _bodies.Length; i++)
            {
                var angle = step * i; // угол в котором будет стоять каждый из элементов
                // рассчитаем позидцию
                //var pos = new Vector2(Mathf.Cos(angle) * _radius, 
                //Mathf.Sin(angle) * _radius);
                // для перемещения добавим _time * _speed
                var pos = new Vector2(Mathf.Cos(angle + _time * _speed) * _radius, 
                    Mathf.Sin(angle + _time * _speed) * _radius);
                // передадим позицию для каждого тела,
                // так мы расставим объекты по кругу (текущая позиция объекта + позиция вложенного объекта в круге)
                //_bodies[i].MovePosition( containerPosition + pos);
                _positions[i] = containerPosition + pos;
            }
        }


        // для отрисовки
#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateContent();
            CalculatePositions();
            
            for (var i = 0; i < _bodies.Length; i++)
            {
                _bodies[i].transform.position = _positions[i];
            }
        }

        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward,_radius);
        }
#endif
    }
}