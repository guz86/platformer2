using System;
using Components;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header("Params")]
    // скорость перемещения влево вправо
    [SerializeField] private float _speed;

    // величина с которой будем прыгать вверх
    //[SerializeField] private float _jumpSpeed;
    [SerializeField] protected float JumpSpeed;

    // величина с которой будем подбрасывать героя при прыгании на пиках
    //[SerializeField] private float _damageJumpSpeed; переименовали
    [SerializeField] private float _damageVelocity;
    
    // урон нашему объекту, которые попал в поле действия меча и имеет ХП
    //[SerializeField] private int _damage;
    
    
    [Header("Checkers")]
    // Ground указываем для поверхностей от которых можно прыгать, на поверхности вешаем слой Ground
    //[SerializeField] private LayerMask _groundLayer;
    [SerializeField] protected LayerMask GroundLayer;

    // Ground указываем для поверхностей от которых можно прыгать, на поверхности вешаем слой Ground
    // [SerializeField] private float _groundCheckRadius;
    //[SerializeField] private Vector3 _groundCheckPoisitionDelta;
    [SerializeField] private LayerCheck _groundCheck;
    
    // переменная для нашей атаке, вызов проверки объектов для атаки GetObjectsInRange
    // добавить на герое наш объект на мече
    [SerializeField] private CheckCircleOverlap _attackRange;
    
    // компонент отвечающий за все партиклы
    [SerializeField] protected SpawnListComponent Particles;
    
    // направление перемещения
    //private Vector2 _direction;
    protected Vector2 Direction;

    // заберем физическое тело
    //private Rigidbody2D _rigidbody;
    protected Rigidbody2D Rigidbody;
    
    //стоим ли земле
    //private bool _isGrounded;
    protected bool IsGrounded;
    
    //private Animator _animator;
    protected Animator Animator;
    // было для разваорота героя private SpriteRenderer _sprite;

    // для fix высокого прыжка
    private bool _isJumping;
    
    
    private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
    private static readonly int IsRunning = Animator.StringToHash("is-running");
    private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
    private static readonly int Hit = Animator.StringToHash("hit");
    // анимация атаки оружием
    private static readonly int AttackKey = Animator.StringToHash("attack");
    
    // через переопределение методов
    protected virtual void Awake()
    {
        // заберем физическое тело
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }
    
    
    // из ввода будет приходить вектор с направлением
    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }
    
    protected virtual void Update()
    {
        // стоим ли на земле? // 
        //_isGrounded = IsGrounded();
        IsGrounded = _groundCheck.isTouchingLayer;
    }
    
    
    // для физического перемещения вычисления в FixedUpdate
    private void FixedUpdate()
    {
        // ПЕРЕМЕЩЕНИЕ, как идем, влево вправо
        var xVelocity = Direction.x * _speed;
        // ПРЫЖОК, как прыгаем
        var yVelocity = CalculateYVelocity();
        // ПЕРЕМЕЩЕНИЕ
        Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

        /* 
        // от длины вектора _rigidbody.velocity будет зависеть скорость и направление перемещения
        // телу задаем скорость в каком то направлении, ставим FreezeRotation z чтобы не вращался
        // old ПЕРЕМЕЩЕНИЕ влево вправо
        _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

        // old прыжок
        // следим за тем нажали ли мы кнопку вверх, изменилась ли координата y
        bool isJumping = _direction.y > 0;
        
        // стоим ли на земле? // 
        var isGround = isGrounded();
         
        if (isJumping)
        {
            //избавляемся от нежелательного поведения с прыжком && _rigidbody.velocity.y <= 0.01f при 0 могут
            // возникнуть проблемы с прыжком с пружинящей поверхности
            if (isGround && _rigidbody.velocity.y <= 0.01f)
            {
                // метод AddForce добавляет силу (куда и как)
                _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            }
        }
        //для высоты прыжка
        // если мы не прыгаем (на нажимаем), но летим вверх, то замедляемся
        else if (_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
        }
        */

        // АНИМАЦИЯ ПЕРЕМЕЩЕНИЯ
        // бежим влево или вправо
        Animator.SetBool(IsRunning, Direction.x != 0);
        // летим вверх или вниз
        Animator.SetFloat(VerticalVelocity, Rigidbody.velocity.y);

        // на земле?
        Animator.SetBool(IsGroundKey, IsGrounded);

        UpdateSpriteDirection();
    }
    
    
    
    
    
    protected virtual float CalculateYVelocity()
    {
        // прыжок - !герой может только прыгать
        // текущее положение
        var yVelocity = Rigidbody.velocity.y;
        // следим за тем нажали ли мы кнопку вверх, изменилась ли координата y
        bool isJumpingPressing = Direction.y > 0;

        //если стоим на земле, то можем сделать двойной прыжок
        if (IsGrounded)
        {
            //_allowDoubleJump = true;
            _isJumping = false;
        }
        
        if (isJumpingPressing)
        {
            _isJumping = true;
            // рассчитаем скорость прыжка
            
            
            // если персонаж падает
            var isFalling = Rigidbody.velocity.y <= 0.01f;
            // если мы не падаем, то не можем прыгать
            //if (!isFalling) return yVelocity;
            
            yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
        }
        //для цепляния к стенам
        // else if (_isOnWall)
        // {
        //     yVelocity = 0;
        //     _allowDoubleJump = true;
        // }
        //для высоты прыжка
        // если мы не прыгаем (на нажимаем), но летим вверх, то замедляемся
        else if (Rigidbody.velocity.y > 0 && _isJumping)
        {
            yVelocity *= 0.5f;
            //_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
        }

        return yVelocity;
    }

    protected virtual float CalculateJumpVelocity(float yVelocity)
    {
        // расчет скорости прыжка

        //избавляемся от нежелательного поведения с прыжком && _rigidbody.velocity.y <= 0.01f при 0 могут
        // возникнуть проблемы с прыжком с пружинящей поверхности
        if (IsGrounded)
        {
            // если не взлетаем то прибавим величину прыжка
            yVelocity += JumpSpeed;
            // анимация прыжка
            //_jumpPaticles.Spawn();
            Particles.Spawn("Jump");
        }
        // else if (_allowDoubleJump)
        // {
        //     yVelocity = _jumpSpeed;
        //     // анимация прыжка
        //     _jumpPaticles.Spawn();
        //     // запретим прыгать 2й раз
        //     _allowDoubleJump = false;
        // }

        return yVelocity;
    }
    
    
    // разворот спрайта
    private void UpdateSpriteDirection()
    {
        // if (_direction.x > 0)
        // {
        //     _sprite.flipX = false;
        // }
        // else if (_direction.x < 0)
        // {
        //     _sprite.flipX = true;
        // }
        //_sprite.flipX = _direction.x < 0 ? true : false;
        // для анимации пыли нужно разворачивать всего героя
        if (Direction.x > 0)
        {
            //transform.localScale = new Vector3(1, 1, 1);
            transform.localScale = Vector3.one;
        }
        else if (Direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    
    // получение урона от пик, вызов анимации, подброс героя вверх
    public virtual void TakeDamage()
    {
        _isJumping = false;
        Animator.SetTrigger(Hit);
        // изменим силу с которой он летит вверх
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageVelocity);

        // // ограничение вылетающих монет при причинении урона герою
        // if (_session.Data.Coins > 0)
        // {
        //     SpawnCoins();
        // }
    }

    public virtual void Attack()
    {
        Animator.SetTrigger(AttackKey);
    }
    
    public void OnAttackKey()
    {
        _attackRange.Check();
        /*// достаем наши объекты с которые атакуем
        var gos = _attackRange.GetObjectsInRange();
        // попробуем у них получить компонент здоровья
        foreach (var go in gos)
        {
            var hp = go.GetComponent<HealthComponent>();
            if (hp != null)
            {
                // если есть здоровье отнимаем значение урона от оружия
                hp.ModifyHealth(-_damage);
            }
        }*/
    }
}
