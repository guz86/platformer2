using UnityEngine;

// компонент сессии будет добавляться на все уровни с дефолтными настройками
public class GameSession : MonoBehaviour
{
    // выделим слой данных
    [SerializeField] private PlayerData _data;
    public PlayerData Data => _data;
    // сохраненная сессия
    private PlayerData _save;
    
    // для сохранения игровой сессии
    private void Awake()
    {
        // если существует сессия, то значит она вторая и нужно уничтожить
        if (IsSessionExit())
        {
            Destroy(gameObject);
        }
        else
        {
            // если сессий нет, то сохраняем (хранилище внутри сцен)
            DontDestroyOnLoad(this);
        }
    }

    // проверка сессии, существует она или нет
    private bool IsSessionExit()
    {
        var sessions = FindObjectsOfType<GameSession>();
        foreach (var gameSession in sessions)
        {
            // если существующая сессия не равна текущему объекту 
            if (gameSession != this)
            {
                return true;
            }
        }
        return false;
    }

    public void Save()
    {
        // нельзя просто передать значение так как PlayerData это объект
        // получаем копию данных(текущих настроек пользователя) и сохраняем ее в _save
        _save = _data.Clone();
    }

    public void LoadLastSave()
    {
        // копируем наоборот
        _data = _save.Clone();
    }
}
