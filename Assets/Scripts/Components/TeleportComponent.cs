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
    
    // для того чтобы получить доступ к партикл системе для отключения
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

    // специальный метод с интефейсом IEnumerator для использования постоянных прерываний
    private IEnumerator AnimateTeleport(GameObject target)
    {
        var input = target.GetComponent<PlayerInput>();
        
        // получим спрайт для которого будем менять альфа
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
    // destAlfa 1 - появление, 0 - исчезновение
    private IEnumerator SetAlfa(SpriteRenderer sprite, float destAlfa)
    {
        // текущее время анимации
        var alfaTime = 0f;
        // дефольтное значение
        var spriteAlfa = sprite.color.a;
        while (alfaTime < _alfaTime)
        {
            alfaTime += Time.deltaTime;
            var progress = alfaTime / _alfaTime;
            // интерполируем(смещаем) от стартогового значения к текущему(целевому)
            var tmpAlfa = Mathf.Lerp(spriteAlfa, destAlfa, progress);
            // возьмем цвет у спрайта
            var color = sprite.color;
            // меняем значение альфа
            color.a = tmpAlfa;
            // запишем новое значение в наш спрайт
            sprite.color = color;
            // вызов пропустит кадр
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
            // у объекта меняем позицию от текущей к целевой
            target.transform.position = Vector3.Lerp(target.transform.position, _destTransform.position, progress);
            // вызов пропустит кадр
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