using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadLevelComponent : MonoBehaviour
{
   public void Reload()
   {
      // после добавление сессии восстанавливаем состояние героя по дефолту
      // выделяем текущую сессию и удаляем
      var session = FindObjectOfType<GameSession>();
      Destroy(session.gameObject);
      
      
      var scene = SceneManager.GetActiveScene();
      SceneManager.LoadScene(scene.name);
   }
}
