using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Components.ColliderBased
{
    public class CheckCircleOverlap : MonoBehaviour
    {
    
        // радиус круга
        [SerializeField] private float _radius = 1f;
        // для фильтрации элементов
        [SerializeField] public LayerMask _mask;
    
        [SerializeField] private OnOverlapEvent _inOverlap;
    
        // будем сортировать по тегам
        [SerializeField] private string[] _tags;

        // массив объектов из 5 элементов
        private readonly Collider2D[] _interactionResult = new Collider2D[10];
    
        /*public GameObject[] GetObjectsInRange()
    {
        // будет создавать круг определенного радиуса
        var size = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            // радиус
            _radius,
            // массив объектов
            _interactionResult);
        
        var overlaps = new List<GameObject>();

        // все объекты вошедшие в круг будут добавляться в массив
        for (int i = 0; i < size; i++)
        {
            overlaps.Add(_interactionResult[i].gameObject);
        }

        return overlaps.ToArray();
    }*/

        // для интерактивной настройки взаимодействия объектов
        private void OnDrawGizmosSelected()
        {
            // задаем цвет для нашего диска
            Handles.color = HandlesUtils.TransparentRed; 
            // диск в центом в transform.position, направление Vector3.forward, 
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }

        public void Check()
        {
            // будем брать объекты и отправлять их в UnityEvent
        
            // будет создавать круг определенного радиуса
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                // радиус
                _radius,
                // массив объектов
                _interactionResult,
                _mask);
        
            // все объекты вошедшие в круг будут добавляться в массив
            for (int i = 0; i < size; i++)
            {
                // фильтрация результатов 
                // если элемент с которым пересеклись,
                // будет иметь тег один из перечисленных, тогда вызываем 
                var overlapResult = _interactionResult[i];
                var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag));
                // будем вызывать результаты нашего пересечения
                if (isInTags)
                {
                    _inOverlap.Invoke(_interactionResult[i].gameObject);
                }
            }
        }

        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject>
        {
        
        }
    }
}