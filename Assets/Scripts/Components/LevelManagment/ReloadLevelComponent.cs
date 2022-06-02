using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components.LevelManagment
{
   public class ReloadLevelComponent : MonoBehaviour
   {
      public void Reload()
      {
         // после добавление сессии восстанавливаем состояние героя по дефолту
         // выделяем текущую сессию и удаляем
         var session = FindObjectOfType<GameSession>();
      
         //Destroy(session.gameObject);
         // не будем удалять текущую сессию а загрузим в нее нашу сохраненную
         session.LoadLastSave();
      
         var scene = SceneManager.GetActiveScene();
         SceneManager.LoadScene(scene.name);
      }
   }
}
