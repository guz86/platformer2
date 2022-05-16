using Cinemachine;
using UnityEngine;

public class CameraStateController : MonoBehaviour
{
    // наша камера StateDrivenCamera
    [SerializeField] private Animator _animator;
    // ShowTargetCamera камера рядом с дверью
    [SerializeField] private CinemachineVirtualCamera _camera;

    // чтобы постоянно не считать хэш, оптимизация
    private static readonly int ShowTargetKey = Animator.StringToHash("ShowTarget");
    
    // передаем позицию нашей камеры к двери
    public void SetPosition(Vector3 targetPosition)
    {
        // координата z всегда висит в одном месте
        targetPosition.z = _camera.transform.position.z;
        // куда поставить камеру
        _camera.transform.position = targetPosition;
    }
    // обновить состояние в Аниматоре
    public void SetState(bool state)
    {
        _animator.SetBool(ShowTargetKey, state);
    }
}
