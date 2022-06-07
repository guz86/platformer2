using UnityEngine;

namespace Components.ColliderBased
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] protected LayerMask Layer;
        [SerializeField] protected bool IsTouchingLayer;
        
        public bool isTouchingLayer => IsTouchingLayer;
    }
}