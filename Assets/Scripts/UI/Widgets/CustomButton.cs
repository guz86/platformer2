using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class CustomButton : Button
    {
        // создали свою кнопку и добавили 2 параметра
        // и включаем и выключаем их в зависимости от состояния кнопки
        
        // в зависимости от состояния кнопки, будем включать один или другой
        [SerializeField] private GameObject _normal;
        [SerializeField] private GameObject _pressed;

        // как поймем что кнопка нажата
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            
            // чтобы не потерять функционал
            base.DoStateTransition(state, instant);
            
            // в зависимости от состояния
            _normal.SetActive(state != SelectionState.Pressed);
            _pressed.SetActive(state == SelectionState.Pressed);
        }
    }
}