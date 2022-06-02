using System;
using UnityEngine;
using UnityEngine.Events;

// обязательно
namespace Components.Animations
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimationClips : MonoBehaviour
    {
        [SerializeField] private int _frameRate = 10;

        [SerializeField] private UnityEvent<string> _onComplete;

        // массив клипов например idle destroy
        [SerializeField] private AnimationClip[] _clips;
    
        // у него будем менять спрайты
        private SpriteRenderer _renderer;

        // сколько раз в секунду менять анимацию, сколько секунд уходит на показ спрайта
        private float _secPerFrame;

        // текущий индекс спрайта из массива
        //private int _currentSpriteIndex;

        // время для следующего обновления текущего спрайта
        private float _nextFrameTime;
    
        private bool _isPlaying = true;
    
        // текущий индекс клипа
        private int _currentClip;
    
        private int _currentFrame;

        private void Start()
        {
            // сколько времени будет длится 1 кадр 
            _secPerFrame = 1f / _frameRate;
            _renderer = GetComponent<SpriteRenderer>();
        }

        // расчет показа следующего кадра
        private void OnEnable()
        {
        
            // текущее время + количество секунд на фрейм = когда следующий апдейт кадра
            //_nextFrameTime = Time.time + _secPerFrame;
        
            // чтобы сразу менялся кадр, выставим текущее время
            _nextFrameTime = Time.time;
            //_currentSpriteIndex = 0;
            // вынесен из старта
            StartAnimation();
        }

        // если объект изсчезает из поля зрения камеры, ты мы его отключаем
        private void OnBecameVisible()
        {
            enabled = _isPlaying;
        }
        private void OnBecameInvisible()
        {
            enabled = false;
        }

        // ищем нужный клип и запускаем анимацию
        public void SetClip(string clipName)
        {
            for (int i = 0; i < _clips.Length; i++)
            {
                if (_clips[i].Name == clipName)
                {
                    _currentClip = i;
                    StartAnimation();
                    return;
                }
            }
            enabled = _isPlaying = false;
        }

        public void StartAnimation()
        {
            //_nextFrameTime = Time.time + _secPerFrame;
            // чтобы сразу менялся кадр, выставим текущее время
            _nextFrameTime = Time.time;
            enabled = _isPlaying = true;
            _currentFrame = 0;
        }
    
        private void Update()
        {
            // если больше текущего времени, то пропустить кадр
            if (_nextFrameTime > Time.time) return;

            // ищем клип
            var clip = _clips[_currentClip];
            if (_currentFrame >= clip.Sprites.Length)
            {
                if (clip.Loop)
                {
                    _currentFrame = 0;
                }
                // иначе переходим на следующий стейт
                else
                {
                    enabled = _isPlaying = clip.AllowNextClip;
                    clip.OnComplete?.Invoke();
                    _onComplete?.Invoke(clip.Name);
                    if (clip.AllowNextClip)
                    {
                        _currentClip = (int) Mathf.Repeat(_currentClip + 1, _clips.Length);
                    }
                }

                return;
            }
        
            // назначаем нужный нам кадр из массива
            _renderer.sprite = clip.Sprites[_currentFrame];
            // увеличиваем время на количество секунд на фрейм
            _nextFrameTime += _secPerFrame;
            // берем следующий спрайт
            _currentFrame++;
        }

        // отдельный класс для каждой анимации
        [Serializable]
        public class AnimationClip
        {
            [SerializeField] private string _name;
            // будет ли повторяться
            [SerializeField] private bool _loop;
            [SerializeField] private Sprite[] _sprites;
            // событие окончание
            [SerializeField] private UnityEvent _onComplete;
            [SerializeField] private bool _allowNextClip;
        
            public string Name => _name;
            public Sprite[] Sprites => _sprites;
            public bool Loop => _loop;
            public bool AllowNextClip => _allowNextClip;
            public UnityEvent OnComplete => _onComplete;
        
        }
    }
}