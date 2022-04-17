using System;
using UnityEngine;
using UnityEngine.Events;

// обязательно
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private int _frameRate;
    // будет ли повторяться
    [SerializeField] private bool _loop;
    [SerializeField] private Sprite[] _sprites;
    // событие окончание
    [SerializeField] private UnityEvent _onComplete;
    
    // у него будем менять спрайты
    private SpriteRenderer _renderer;

    // сколько раз в секунду менять анимацию, сколько секунд уходит на показ спрайта
    private float _secondPerFrame;

    // текущий индекс спрайта из массива
    private int _currentSpriteIndex;

    // время для следующего обновления текущего спрайта
    private float _nextFrameTime;
    //private bool _isPlaying = true;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        
        // сколько времени будет длится 1 кадр 
        _secondPerFrame = 1f / _frameRate;
        // текущее время + количество секунд на фрейм = когда следующий апдейт кадра
        _nextFrameTime = Time.time + _secondPerFrame;
        _currentSpriteIndex = 0;
    }

    private void Update()
    {
        // если больше текущего времени, то пропустить кадр
        if (_nextFrameTime > Time.time) return;

        // если наступило время для смены кадра
        // чтобы не выйти за границы массива спрайтов
        if (_currentSpriteIndex >= _sprites.Length)
        {
            // если установлен флаг цикличности
            if (_loop)
            {
                _currentSpriteIndex = 0;
            }
            else
            {
                enabled = false; // выключить
                _onComplete?.Invoke();
                return;
            }
        }
            // назначаем нужный нам кадр из массива
            _renderer.sprite = _sprites[_currentSpriteIndex];
            // увеличиваем время на количество секунд на фрейм
            _nextFrameTime += _secondPerFrame;
            // берем следующий спрайт
            _currentSpriteIndex++;
    }
}