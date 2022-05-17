using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckCircleOverlap : MonoBehaviour
{
    
    // радиус круга
    [SerializeField] private float _radius = 1f;

    // массив объектов из 5 элементов
    private readonly Collider2D[] _interactionResult = new Collider2D[5];
    
    public GameObject[] GetObjectsInRange()
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
    }

    // для интерактивной настройки взаимодействия объектов
    private void OnDrawGizmosSelected()
    {
        // задаем цвет для нашего диска
        Handles.color = HandlesUtils.TransparentRed; 
        // диск в центом в transform.position, направление Vector3.forward, 
        Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
    }
}