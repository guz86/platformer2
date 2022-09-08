using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MainMenu
{
    // обрабатывает клики на кнопки меню

    public class MainMenuWindow : AnimatedWindow
    {
        private Action _closeAction;

        public void OnShowSettings()
        {
            // путь к префабам UI
            // загружаем наш префаб, находим канвас и в нем создаем объект префаба
            var window = Resources.Load<GameObject>("UI/SettingsWindow");
            var canvas = FindObjectOfType<Canvas>();
            Instantiate(window, canvas.transform);
        }

        // сработают (анонимные функции) после проигрывания анимации

        public void OnStartGame()
        {
            // помещаем код анонимной функции загрузка нового уровня
            _closeAction = () => { SceneManager.LoadScene("Level1"); };
            Close();
        }

        public void OnExit()
        {
            // помещаем код анонимной функции закрытия приложения, или остановки
            _closeAction = () =>
            {
                Application.Quit();

                //для редактора остановим проигрывание
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            // вызываем по нажатию на кнопку действие и убираем окно
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
        }
    }
}