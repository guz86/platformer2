using UnityEngine;

namespace Creatures.Weapons
{
    public class SinusProjectile : BaseProjectile
    {
        [SerializeField] private float _frequency = 20f;
        [SerializeField] private float _amplitude = 0.2f;
        private float _originalY;
        private float _time;

        protected override void Start()
        {
            base.Start();
            _originalY = Rigidbody.position.y;
        }

        private void FixedUpdate()
        {
            // заберем позицию
            var position = Rigidbody.position;
            // периодическое движение по синусу
            position.x += Direction * Speed;
            //position.y =_originalY + Mathf.Sin(_time);
            // поменяем аплитуду и частоту
            position.y =_originalY + Mathf.Sin(_time * _frequency) * _amplitude;
            Rigidbody.MovePosition(position);
            // чтобы вылетал из одного места жемчужина
            _time += Time.fixedDeltaTime;
        }
    }
}