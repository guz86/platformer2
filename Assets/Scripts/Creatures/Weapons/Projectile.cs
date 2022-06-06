using System;
using UnityEngine;

namespace Creatures.Weapons
{
    public class Projectile : BaseProjectile
    {
        
        protected override void Start()
        {
            base.Start();
            
            // второй вариант
            // не двигать а прикладывать к объекту силу
            // на SwordProjectile в RigidBody BodyType - ставим Dinamic, чтобы на него действовали силы
            // чтобы он летел просто прямо GravityScale 0 (например если от 0 до 1 то будет падать)
            var forse = new Vector2(Direction * Speed, 0);
            // скорость поставим _speed = 5
            Rigidbody.AddForce(forse,ForceMode2D.Impulse);
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