using System.Collections;
using UnityEngine;

public class TeleportComponent : MonoBehaviour
{
    // точка для телепортации, добавим пустой объект на сцене
    [SerializeField] private Transform _destTransform;

    // время исчезновения
    [SerializeField] private float _alfaTime = 1;
    
    // время передвижения
    [SerializeField] private float _moveTime = 1;

    public void Teleport(GameObject target)
    {
        // мгновенный перенос
        //target.transform.position = _destTransform.position;
        StartCoroutine(AnimateTeleport(target));
    }

    private IEnumerator AnimateTeleport(GameObject target)
    {
        // исчезновение
        var sprite = target.GetComponent<SpriteRenderer>();
        var alfaTime = 0f;
        var spriteAlfa = sprite.color.a;
        while (alfaTime < _alfaTime)
        {
            alfaTime += Time.deltaTime;

            var tmpAlfa = Mathf.Lerp(spriteAlfa, 0, alfaTime / _alfaTime);
            var color = sprite.color;
            color.a = tmpAlfa;
            sprite.color = color;

            yield return null;
        }
        
        // перенос
        var moveTime = 0f;
        while (moveTime < _moveTime)
        {
            moveTime += Time.deltaTime;
            
            target.transform.position = Vector3.Lerp(target.transform.position, _destTransform.position,
                moveTime/_moveTime);

            yield return null;
        }
        
        
        // появление
        
        
    }

}