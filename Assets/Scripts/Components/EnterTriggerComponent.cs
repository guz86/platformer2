using UnityEngine;

public class EnterTriggerComponent : MonoBehaviour
{
     [SerializeField] private string _tag;
     // объект для сериализации методов
     //[SerializeField] private UnityEvent _action;
     [SerializeField] private EnterEvent _action;

     private void OnTriggerEnter2D(Collider2D other)
     {
          if (other.gameObject.CompareTag(_tag)) 
          {
               _action?.Invoke(other.gameObject);
          } 
     }
}
