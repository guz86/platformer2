using UnityEngine;
using UnityEngine.Events;

public class InteractiveComponent : MonoBehaviour
{
    // действие в случае вызова метода из компонента
    [SerializeField] private UnityEvent _action;

    // Interact() будет вызываться из героя
    public void Interact()
    {
        _action?.Invoke();
    }

}
