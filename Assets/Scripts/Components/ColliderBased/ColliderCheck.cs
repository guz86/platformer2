using UnityEngine;

namespace Components.ColliderBased
{
    public class ColliderCheck : LayerCheck
    {
       // [SerializeField] private LayerMask Layer;
      //  [SerializeField] private bool IsTouchingLayer;
   
        private Collider2D _collider;

        //public bool isTouchingLayer => IsTouchingLayer;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        // пересечение слоев
        private void OnTriggerStay2D(Collider2D other)
        {
            // соприкосается ли коллайдер с указанным слоем
            IsTouchingLayer = _collider.IsTouchingLayers(Layer);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            IsTouchingLayer = _collider.IsTouchingLayers(Layer);
        }
    }
}