using UnityEngine;

namespace Components.GoBased
{
    public class GoContainerComponent : MonoBehaviour
    {
        // объекты которые будем выстреливать, их нужно послать в RandomSpawner
        [SerializeField] private GameObject[] _gos;
        [SerializeField] private DropEvent _onDrop;
        
        [ContextMenu("Drop")]
        public void Drop()
        {
            _onDrop.Invoke(_gos);
        }
    }
}