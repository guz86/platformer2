using UnityEngine;


public class LayerCheck : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;
    private Collider2D _collider;

    public bool isTouchingLayer;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    // пересечение слоев
    private void OnTriggerStay2D(Collider2D other)
    {
        // соприкосается ли коллайдер с указанным слоем
        isTouchingLayer = _collider.IsTouchingLayers(_layer);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isTouchingLayer = _collider.IsTouchingLayers(_layer);
    }
}