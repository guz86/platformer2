using UnityEngine;

public class DestroyObjectComponent : MonoBehaviour
{
    [SerializeField] private GameObject _objectToDestroy;
    [SerializeField] private Hero _hero;
    
    public void DestroyObject()
    {
        Destroy(_objectToDestroy);
    }
}
