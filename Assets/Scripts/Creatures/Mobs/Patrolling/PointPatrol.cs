using System.Collections;
using UnityEngine;

namespace Creatures.Mobs.Patrolling
{
    public class PointPatrol : Patrol
    {
        // патрулировать будем по точкам
        [SerializeField] private Transform[] _points;
        
        // разница, когда приблизились достаточно
        [SerializeField] private float _treshold = 1f;

        private Creature _creature;
        private int _destinationPointIndex;
        
        private void Awake()
        {
            // получаем для того чтобы управлять им в будущем
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator DoPatrol()
        {
            // если уже находимся на точке, то должны перейти на следующую
            while (enabled)
            {
                if (IsOnPoint())
                {
                    // 0 1 0 1 0 1 меняем точку
                    _destinationPointIndex = (int) Mathf.Repeat(_destinationPointIndex+1, _points.Length);
                }
                
                // направляем моба до нашей точки
                var direction = _points[_destinationPointIndex].position - transform.position;
                direction.y = 0;
                // указываем в какую сторону идти
                _creature.SetDirection(direction.normalized);
                yield return null;
            }
        }

        private bool IsOnPoint()
        {
            // сравнить текущую позицию с позицией до точки
            // magnitude - длина вектора
            // если _treshold больше чем длина вектора до нашей точки, значит мы дошли
            return (_points[_destinationPointIndex].position - transform.position).magnitude < _treshold;

        }
    }
}