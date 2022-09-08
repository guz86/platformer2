using UnityEngine;

namespace UI
{
    public class AnimatedWindow : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Show = Animator.StringToHash("Show");
        private static readonly int Hide = Animator.StringToHash("Hide");

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.SetTrigger(Show);
        }

        public void Close()
        {
            _animator.SetTrigger(Hide);
        }

        // т.к. Hide операция не синхронная выполняется какоето время
        // уничтожить окно после закрытия, вызов будет из Event в Hide анимации
        //virtual чтобы можно было переопределять
        public virtual void OnCloseAnimationComplete()
        {
            Destroy(gameObject);
        }
    }
}