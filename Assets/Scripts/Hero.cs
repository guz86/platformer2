using System;
using UnityEngine;

public class Hero : MonoBehaviour
{
    // скорость перемещения влево вправо
    [SerializeField] private float _speed;
    // величина с которой будем прыгать вверх
    [SerializeField] private float _jumpSpeed;
    // Ground указываем для поверхностей от которых можно прыгать, на поверхности вешаем слой Ground
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private Vector3 _groundCheckPoisitionDelta;

    

    // направление перемещения
    private Vector2 _direction;
    // заберем физическое тело
    private Rigidbody2D _rigidbody;
    //стоим ли земле
    private bool _isGrounded;
    // можно ли делать двойной прыжок
    private bool _allowDoubleJump;
    
    private Animator _animator;
    private SpriteRenderer _sprite;

    private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
    private static readonly int IsRunning = Animator.StringToHash("is-running");
    private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");

    public int coints;

    private void Awake()
    {
        // заберем физическое тело
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        
        // стоим ли на земле? // 
        _isGrounded = IsGrounded();
    }

    // для физического перемещения вычисления в FixedUpdate
    private void FixedUpdate()
    { 
        // ПЕРЕМЕЩЕНИЕ, как идем, влево вправо
        var xVelocity = _direction.x * _speed;
        // ПРЫЖОК, как прыгаем
        var yVelocity = CalculateYVelocity();
        // ПЕРЕМЕЩЕНИЕ
        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
        
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
        _animator.SetBool(IsRunning, _direction.x != 0);
        // летим вверх или вниз
        _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);

        // на земле?
        _animator.SetBool(IsGroundKey, _isGrounded);

        UpdateSpriteDirection();
    }

    private float CalculateYVelocity()
    {
        // прыжок
        // текущее положение
        var yVelocity = _rigidbody.velocity.y;
        
        //если стоим на земле, то можем сделать двойной прыжок
        if (_isGrounded) _allowDoubleJump = true;
        
        // следим за тем нажали ли мы кнопку вверх, изменилась ли координата y
        bool isJumpingPressing = _direction.y > 0;
        
        if (isJumpingPressing)
        {
            // рассчитаем скорость прыжка
            yVelocity = CalculateJumpVelocity(yVelocity);
        }
        //для высоты прыжка
        // если мы не прыгаем (на нажимаем), но летим вверх, то замедляемся
        else if (_rigidbody.velocity.y > 0)
        {
            yVelocity *= 0.5f;
            //_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
        }
        return yVelocity;
    }

    private float CalculateJumpVelocity(float yVelocity)
    {
        // расчет скорости прыжка
        // если персонаж падает
        var isFalling = _rigidbody.velocity.y <= 0.01f;
        // если мы не падаем, то не можем прыгать
        if (!isFalling) return yVelocity;
        
        //избавляемся от нежелательного поведения с прыжком && _rigidbody.velocity.y <= 0.01f при 0 могут
        // возникнуть проблемы с прыжком с пружинящей поверхности
        if (_isGrounded)
        {
            // если не взлетаем то прибавим величину прыжка
            yVelocity += _jumpSpeed;
        }
        else if (_allowDoubleJump)
        {
            yVelocity = _jumpSpeed;
            // запретим прыгать 2й раз
            _allowDoubleJump = false;
        }
        
        return yVelocity;
    }

    // из ввода будет приходить вектор с направлением
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    //пересекается ли объект с землей
    private bool IsGrounded()
    {
        //луч
        //var hit = Physics2D.Raycast(transform.position, Vector2.down,1,_groundLayer);
        //сфера
        var hit = Physics2D.CircleCast(transform.position + _groundCheckPoisitionDelta, _groundCheckRadius,
            Vector2.down, 0, _groundLayer);
        return hit.collider != null;
    }
    
    //отслеживание пересечения с землей
    private void OnDrawGizmos()
    {
        //Debug.DrawRay(transform.position,Vector3.down, isGrounded() ? Color.green : Color.red);
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position + _groundCheckPoisitionDelta, _groundCheckRadius);
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
         _sprite.flipX = _direction.x < 0 ? true : false;
    }

    
}