using System;
using UnityEngine;

namespace Components.ColliderBased
{
    public class LineCheck : LayerCheck
    {
        [SerializeField] private Transform _target;

        private readonly RaycastHit2D[] _result = new RaycastHit2D[1];
        
        private void Update()
        {
            // от текущей позиции до _target.position будем бросать линию,
            // будем смотреть пересекается с землей или нет
            IsTouchingLayer = Physics2D.LinecastNonAlloc(transform.position,
                _target.position,
                           _result,
                        Layer) > 0;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.DrawLine(transform.position, _target.position);
        }
#endif
    }
}