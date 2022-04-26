using Unity.Mathematics;
using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
    [SerializeField] private Transform _target;
    // объект с анимацией пыли
    [SerializeField] private GameObject _prefab;

    // метод будет создавать копию префаба
    [ContextMenu("Spawn")]
    public void Spawn()
    {
        // _prefab то что колнируем, _target.position позиция в мире относительно героя, будет оставаться на месте
        var instantiate = Instantiate(_prefab, _target.position, Quaternion.identity);
        // получим instantiate и в нем поменяем расположение Scale, для разворота спрайта пыли
        instantiate.transform.localScale = _target.lossyScale;
    }
}
