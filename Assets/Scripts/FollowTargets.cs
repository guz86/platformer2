using System;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowTargets : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void LateUpdate()
    {
        var destination = new Vector3(_target.position.x, _target.position.y,transform.position.z);
        transform.position = destination;
    }
}
