using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components.LevelManagment
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void Exit()
        {
            // нужно найти сессию
            var session = FindObjectOfType<GameSession>();
            session.Save();
            // загружаем новую сцену 
            SceneManager.LoadScene(_sceneName);
        }
    }
}
