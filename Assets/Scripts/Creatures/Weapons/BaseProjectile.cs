using UnityEngine;

namespace Creatures.Weapons
{
    public class BaseProjectile : MonoBehaviour
    {
        // класс будет заставлять лететь меч
        // 1 вариант
        // скорость палета
        [SerializeField] protected float Speed;

        protected Rigidbody2D Rigidbody;
        
        // для учета направления полета
        protected int Direction;
        
        // для разворота полета выстрела от ракушки жемчужиной
        [SerializeField] private bool _invertX;
        
        protected virtual void Start()
        {
            // 1 вариант
            // на объект SwordProjectile добавим CircleCollider для расчета столкновений
            // на объект SwordProjectile добавим rigidbody2d
            // BodyType - Kinematic и его позиция будет ресчитываться скриптом
            // CollisionDetection - Continius - чтобы четко втыкался в стены(перелет, недолет)
            // interpolate - Interpolate чтобы объект летел более гладко
            // получим rigidbody которому будем изменять позицию
            Rigidbody = GetComponent<Rigidbody2D>();

            
            //+ мы будем менять направление в зависимости от флага, для жемчужины
            var mod = _invertX ? -1 : 1;
            
            // lossyScale задаем именно в старте, потому что уже будет известно о позиции героя
            Direction = mod * transform.lossyScale.x > 0 ? 1 : -1;
            
            
            // второй вариант
            // не двигать а прикладывать к объекту силу
            // на SwordProjectile в RigidBody BodyType - ставим Dinamic, чтобы на него действовали силы
            // чтобы он летел просто прямо GravityScale 0 (например если от 0 до 1 то будет падать)
            //var forse = new Vector2(Direction * Speed, 0);
            // скорость поставим _speed = 5
            //Rigidbody.AddForce(forse,ForceMode2D.Impulse);
        }

        // 1 вариант
        // private void FixedUpdate()
        // {
        //     // передвижение = получим текущую позицию и прибавим к ней скорость
        //     var position = _rigidbody.position;
        //     //position.x += _speed;
        //     // _direction задаем направление влево или вправо
        //     position.x += _direction * _speed;
        //     // переместим
        //     _rigidbody.MovePosition(position);
        // }
        
    }
}