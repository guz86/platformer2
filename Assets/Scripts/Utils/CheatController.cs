using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Utils
{
    public class CheatController : MonoBehaviour
    {
        // ввод будем сохранять в какую-то строку, строка будет жить какое-то время
        private string _currentInput;

        // переменная которая будет сбрасывать текущий Input
        private float _inputTime;

        //строка будет жить какое-то время
        [SerializeField] private float _inputTimeToLive;

        // нужно где то отдельно хранить читы, это будет отдельный объект
        [SerializeField] private CheatItems[] _cheats;

        private void Awake()
        {
            // нужно получить ввод с клавиатуры, добавим метод обработчик OnTextInput
            // подписываемся на ввод текста с клавиатуры
            Keyboard.current.onTextInput += OnTextInput;
        }

        private void OnDestroy()
        {
            // отписываеся
            Keyboard.current.onTextInput -= OnTextInput;
        }

        private void OnTextInput(char InputChar)
        {
            _currentInput += InputChar;
            // будем сбрасывать до заданного значения, каждый раз когда нажмем кнопку
            _inputTime = _inputTimeToLive;
            // обработка самой строки
            FindAnyCheats();
        }

        private void FindAnyCheats()
        {
            // пройдемся по всем читам
            foreach (var cheatItem in _cheats)
            {
                // если строка содержит название чита, то вызываем действие и сбросим строку
                if (_currentInput.Contains(cheatItem.Name))
                {
                    cheatItem.Action.Invoke();
                    _currentInput = string.Empty;
                }
            }
        }

        private void Update()
        {
            // сбрасывание строки
            if (_inputTime < 0)
            {
                _currentInput = string.Empty;
            }
            else
            {
                // от заданного времени отнимаем время прошедшее с отображения кадра
                _inputTime -= Time.deltaTime;
            }
        }

        //чтобы можно было использовать поля в качестве [SerializeField]
        [Serializable]
        // создадим отдельный класс для читов
        // using UnityEngine.Events;
        public class CheatItems
        {
            public string Name;
            public UnityEvent Action; // что должны сделать в ответ
        }
    }
}