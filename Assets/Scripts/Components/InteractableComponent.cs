using UnityEngine;
using UnityEngine.Events;

public class InteractableComponent : MonoBehaviour
{
    // действие в случае вызова метода из компонента
    [SerializeField] private UnityEvent _action;

    // Interact() будет вызываться из героя
    public void Interact()
    {
        _action?.Invoke();
    }

}
