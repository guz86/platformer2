using System;
using Model.Data;
using Model.Data.Properties;
using UnityEngine;

namespace Components.Audio
{
    // для проигрывания музыки и звуков свяжем audiosource и сеттинги
    // добавленный компонент будет указывать какой громкости должны быть наши сорсы
    // создадим объекты в сервисных SfxAudioSource( добавим отдельный tag)
    // и MusicAudioSource

    [RequireComponent(typeof(AudioSource))]
    public class AudioSettingsComponent : MonoBehaviour
    {
  
        [SerializeField] private SoundSettings _mode;
        
        private FloatPersistentProperty _model;
        private AudioSource _source;
        
        private void Start()
        {
            _source = GetComponent<AudioSource>();
            _model = FindProperty();
            _model.OnChanged += OnSoundSettingChanged;
            // вызовем на старте чтобы установить значения
            OnSoundSettingChanged(_model.Value, _model.Value);
        }

        private void OnSoundSettingChanged(float newValue, float oldValue)
        {
            _source.volume = newValue;
        }

        // возвращаем нужное ном Property
        private FloatPersistentProperty FindProperty()
        {
            switch (_mode)
            {
                case SoundSettings.Music:
                    return GameSettings.I.Music;
                case SoundSettings.Sfx:
                    return GameSettings.I.Sfx;
            }

            throw new ArgumentException("Undefind mode");
        }
        
        // обязательно отписаться от всех событий на котоыре подписаны
        private void OnDestroy()
        {
            _model.OnChanged -= OnSoundSettingChanged;
        }
    }
}