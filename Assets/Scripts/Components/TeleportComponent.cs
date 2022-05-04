using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportComponent : MonoBehaviour
{
    // точка для телепортации, добавим пустой объект на сцене
    [SerializeField] private Transform _destTransform;

    // время исчезновения
    [SerializeField] private float _alfaTime = 1;

    // время передвижения
    [SerializeField] private float _moveTime = 1;
    
    // для того чтобы получить доступ к партикл системе
    private Hero _hero;
    // для того чтобы получить доступ к партикл системе
    private void Start()
    {
        _hero = FindObjectOfType<Hero>();
    }
    
    public void Teleport(GameObject target)
    {
        // мгновенный перенос
        //target.transform.position = _destTransform.position;
        StartCoroutine(AnimateTeleport(target));
    }

    private IEnumerator AnimateTeleport(GameObject target)
    {
        
        var input = target.GetComponent<PlayerInput>();
        var sprite = target.GetComponent<SpriteRenderer>();
        
        // отключаем управление у Hero до исчезновения, т.к. известно что реализация у нас через PlayerInput
        SetLockInput(input, true);
        
        // исчезновение
        yield return SetAlfa(sprite, 0);

        // выключаем объект, чтобы не взаимодействовал с окружением
        target.SetActive(false);

        // перенос
        yield return MoveAnimation(target);

        // включаем объект
        target.SetActive(true);

        // отключим парикл систему перед появлением // временное решение чтобы не спамить коинами
        var _hitParticles = _hero.GetHitParticleSystem();
        _hitParticles.gameObject.SetActive(false);
        
        // появление
        yield return SetAlfa(sprite, 1);

        // включаем управление
        SetLockInput(input, false);
    }


    // отдельный метод для появления/исчезновения через карутину
    private IEnumerator SetAlfa(SpriteRenderer sprite, float destAlfa)
    {
        var alfaTime = 0f;
        var spriteAlfa = sprite.color.a;
        while (alfaTime < _alfaTime)
        {
            alfaTime += Time.deltaTime;
            var progress = alfaTime / _alfaTime;
            var tmpAlfa = Mathf.Lerp(spriteAlfa, destAlfa, progress);
            var color = sprite.color;
            color.a = tmpAlfa;
            sprite.color = color;

            yield return null;
        }
    }

    // отдельный метод для переноса
    private IEnumerator MoveAnimation(GameObject target)
    {
        var moveTime = 0f;
        while (moveTime < _moveTime)
        {
            moveTime += Time.deltaTime;
            var progress = moveTime / _moveTime;
            target.transform.position = Vector3.Lerp(target.transform.position, _destTransform.position, progress);

            yield return null;
        }
    }

    // отдельный метод отключения управления
    private void SetLockInput(PlayerInput input, bool isLocked)
    {
        if (input != null)
        {
            input.enabled = !isLocked;
        }
    }
}