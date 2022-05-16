using UnityEngine;

// то что будем вешать на сам переключатель
public class ShowTargetComponent : MonoBehaviour
{
    // куда будем смотреть, наша дверь
    [SerializeField] private Transform _target;
    // дополнительная камера StateDrivenCamera
    [SerializeField] private CameraStateController _controller;
    // для возврата время ожидания
    [SerializeField] private float _delay = 2f;
    
    // чтобы не дергать на Aweke, найдем контроллер
    private void OnValidate()
    {
        if (_controller != null)
        {
            _controller = FindObjectOfType<CameraStateController>();
        }
    }

    // будет висеть на переключателе
    public void ShowTarget()
    {
        _controller.SetPosition(_target.position);
        _controller.SetState(true);
        // возврат назад
        Invoke(nameof(MoveBack),_delay);
    }
    
    //для возврата
    private void MoveBack()
    {
        _controller.SetState(false);
    }
}
