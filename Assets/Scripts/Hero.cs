using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private Vector3 _groundCheckPoisitionDelta;
    
    
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private Animator _animator;
    private SpriteRenderer _sprite;

    private static readonly int IsGround = Animator.StringToHash("is-ground");
    private static readonly int IsRunning = Animator.StringToHash("is-running");
    private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");

    
    public int coints;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction.x * _speed,_rigidbody.velocity.y);
        
        // прыжок
        bool isJumping = _direction.y > 0;
        var isGround = isGrounded();
        if (isJumping)
        {
            //избавляемся от нежелательного поведения с прыжком && _rigidbody.velocity.y <= 0.1f при 0 могут
            // возникнуть проблемы с прыжком с пружинящей поверхности
            if (isGround && _rigidbody.velocity.y <= 0.1f)
            {
                _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            } 
        }
        //для высоты прыжка
        // если мы не прыгаем (на нажимаем), но летим вверх, то замедляемся
        else if (_rigidbody.velocity.y > 0 )
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
        }
        // бежим влево или вправо
        _animator.SetBool(IsRunning, _direction.x != 0);
        // летим вверх или вниз
        _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);
        // на земле?
        _animator.SetBool(IsGround, isGround);

        UpdateSpriteDirection();
    }

    private void UpdateSpriteDirection()
    {
        if (_direction.x > 0)
        {
            _sprite.flipX = false;
        }
        else if (_direction.x < 0)
        {
            _sprite.flipX = true;
        }
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    
    //пересекается ли объект с землей
    private bool isGrounded()
    {
        //луч
        //var hit = Physics2D.Raycast(transform.position, Vector2.down,1,_groundLayer);
        //сфера
         var hit = Physics2D.CircleCast(transform.position +_groundCheckPoisitionDelta, _groundCheckRadius,Vector2.down,0, _groundLayer);
         return hit.collider != null;
        
    }
    
    //отслеживание
    private void OnDrawGizmos()
    {
        //Debug.DrawRay(transform.position,Vector3.down, isGrounded() ? Color.green : Color.red);
        Gizmos.color = isGrounded() ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position + _groundCheckPoisitionDelta, _groundCheckRadius);
    }
    
    
}
