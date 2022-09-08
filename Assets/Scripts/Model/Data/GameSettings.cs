using Model.Data.Properties;
using UnityEngine;

namespace Model.Data
{
    // через юнити создадим файл в папке
    [CreateAssetMenu(menuName = "Data/GameSettings", fileName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        // переменные отвечающие за сохранение значений
        // [SerializeField] private float _music;
        // [SerializeField] private float _sfx;

        //после реализации FloatPersistentProperty
        [SerializeField] private FloatPersistentProperty _music;
        [SerializeField] private FloatPersistentProperty _sfx;

        //для доступа в SettingsWindow
        public FloatPersistentProperty Music => _music;
        public FloatPersistentProperty Sfx => _sfx;
        
        // ресурсный singleton
        private static GameSettings _instance;
        public static GameSettings I => _instance == null ? LoadGameSettings() : _instance;

        private static GameSettings LoadGameSettings()
        {
            return _instance = Resources.Load<GameSettings>("GameSettings");
        }


        private void OnEnable()
        {
            _music = new FloatPersistentProperty(1, SoundSettings.Music.ToString());
            _sfx = new FloatPersistentProperty(1, SoundSettings.Sfx.ToString());
        }
        // для сохранения их в постоянную память PlayerPrefs для одной переменной
        // private void OnEnable()
        // {
        //     PlayerPrefs.SetFloat("music", 20f);
        //     PlayerPrefs.Save();
        // }
        
        // получать изменения при передвижении ползунка в редакторе
        private void OnValidate()
        {
            _music.Validate();
            _sfx.Validate();
        }
    }

    public enum SoundSettings
    {
        Music,
        Sfx
    }
}