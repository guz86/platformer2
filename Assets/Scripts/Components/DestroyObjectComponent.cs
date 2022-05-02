using System;
using UnityEngine;

public class DestroyObjectComponent : MonoBehaviour
{
    [SerializeField] private GameObject _objectToDestroy;
    private Hero _hero;

    private void Start()
    {
        _hero = FindObjectOfType<Hero>();
    }

    public void DestroyObject()
    {
        Destroy(_objectToDestroy);
    }
}
