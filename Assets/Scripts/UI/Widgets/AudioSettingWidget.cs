using Model.Data.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    // привязывается к виджетам музыки и эффектов
    // абстракция и отдельный виджет для слайдеров
    public class AudioSettingWidget : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _value;

        private FloatPersistentProperty _model;

        private void Start()
        {
            
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        
        
        // инициализация модели
        public void SetModel(FloatPersistentProperty model)
        {
            _model = model;
            // подпишемся и обновим значение
            // обновление значения в одну сторону
            model.OnChanged += OnValueChanged;
            OnValueChanged(model.Value, model.Value);
        }
        

        private void OnSliderValueChanged(float value)
        {
            //обновим значение со слайдера
            _model.Value = value;
        }

        // обновление значения в одну сторону
        private void OnValueChanged(float newValue, float oldValue)
        {
            var textValue = Mathf.Round(newValue * 100);
            _value.text = textValue.ToString();
            _slider.normalizedValue = newValue;
        }

        // обязательно отписаться от всех событий на котоыре подписаны
        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            _model.OnChanged -= OnValueChanged;
        }
    }
}