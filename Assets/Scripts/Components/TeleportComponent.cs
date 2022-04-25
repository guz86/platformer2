using UnityEngine;

public class TeleportComponent : MonoBehaviour
{
   // точка для телепортации, добавим пустой объект на сцене
   [SerializeField] private Transform _destTransform;

   public void Teleport(GameObject target)
   {
      target.transform.position = _destTransform.position;
   }
}
