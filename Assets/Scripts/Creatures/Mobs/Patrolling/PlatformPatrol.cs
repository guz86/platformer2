using System.Collections;
using Components.ColliderBased;
using UnityEngine;

namespace Creatures.Mobs.Patrolling
{
    public class PlatformPatrol : Patrol
    {
        //[SerializeField] private ColliderCheck _groundCheck;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private LayerCheck _obstacleCheck; // препятствие
        [SerializeField] private int _direction;
        [SerializeField] private Creature _creature;

        
        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                // идем в сторону либо разворачиваемся
                // если не соприкосамся с землей впереди моба
                
                // если идем по земле и не соприкосаемся с предметами впереди то идем вперед
                if (_groundCheck.isTouchingLayer && !_obstacleCheck.isTouchingLayer)
                {
                    _creature.SetDirection(new Vector2(_direction, 0));
                }
                else
                {
                    _direction = -_direction;
                    _creature.SetDirection(new Vector2(_direction, 0));
                }

                yield return null; // пропустим кадр
            }
        }
    }
}