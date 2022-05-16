using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// первый таймер запускается через Interactable компонент

public class TimerComponent : MonoBehaviour
{
    // на объекте может быть несколько таймеров
    [SerializeField] private TimerData[] _timers;

    public void SetTimer(int index)
    {
        // передаем наш id таймера
        var timer = _timers[index];
        StartCoroutine(StartTimer(timer));
    }

    private IEnumerator StartTimer(TimerData timer)
    {
        // ждем определенное количество времени
        yield return new WaitForSeconds(timer.Delay);
        // вызываем наш колбэк
        timer.OnTimerUp?.Invoke();
    }
    
    [Serializable]
    class TimerData
    {
        // через сколько таймер стартанет
        public float Delay;
        // что запустим наш колбэк 
        // 1 таймер ShowTarget, запуск второго таймера
        // 2 таймер Switch переключение двери
        public UnityEvent OnTimerUp;
    }  
}
 