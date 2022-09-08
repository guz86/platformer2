using Model.Data;
using UI.Widgets;
using UnityEngine;

namespace UI.Settings
{
    // добавляем к SettingsWindow и передаем туда наши виджеты
    // привязка модели данные к контролеру UI
    public class SettingsWindow : AnimatedWindow
    {
        [SerializeField] private AudioSettingWidget _music;
        [SerializeField] private AudioSettingWidget _sfx;

        protected override void Start()
        {
            base.Start();
            //GameSettings.I.Music

            _music.SetModel(GameSettings.I.Music);
            _sfx.SetModel(GameSettings.I.Sfx);
        }
    }
}