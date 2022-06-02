using UnityEngine;

namespace Components.Interactions
{
    public class SwitchComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        // влкючен или выключен аниматор
        [SerializeField] private bool _state;
        [SerializeField] private string _animationKey;

        public void Switch()
        {
            _state = !_state;
            // меняем состояние и в аниматор передаем
            _animator.SetBool(_animationKey,_state);
        }

        [ContextMenu("Switch")]
        public void SwitchIt()
        {
            Switch();
        }
    }
}
