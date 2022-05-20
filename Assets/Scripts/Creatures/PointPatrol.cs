using System;
using System.Collections;
using UnityEngine;

namespace Creatures
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
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator DoPatrol()
        {
            // если уже находимся на точке, то должны перейти на следующую
            while (enabled)
            {
                if (IsOnPoint())
                {
                    _destinationPointIndex = (int) Mathf.Repeat(_destinationPointIndex+1, _points.Length);
                }
                
                // направляем моба до нашей точки
                var direction = _points[_destinationPointIndex].position - transform.position;
                direction.y = 0;
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