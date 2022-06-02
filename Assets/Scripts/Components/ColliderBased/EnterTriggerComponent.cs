using UnityEngine;
using Utils;

namespace Components.ColliderBased
{
     public class EnterTriggerComponent : MonoBehaviour
     {
          [SerializeField] private string _tag;
     
          // добавим возможность проверки по Layer
          // все включено ~0
          [SerializeField] private LayerMask _layer = ~0;
     
          // объект для сериализации методов
          //[SerializeField] private UnityEvent _action;
          [SerializeField] private EnterEvent _action;

          private void OnTriggerEnter2D(Collider2D other)
          {
               // если _layer не совпадает с нужным
               if (!other.gameObject.IsInLayer(_layer)) return;

               //если есть _tag и он не тот который мы хотим
               if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag)) return;
          
               //if (other.gameObject.CompareTag(_tag)) 
          
               _action?.Invoke(other.gameObject);
          }
     }
}
