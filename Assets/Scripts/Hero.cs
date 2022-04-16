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
    public int coints;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction.x * _speed,_rigidbody.velocity.y);
        
        // прыжок
        bool isJumping = _direction.y > 0;
        if (isJumping)
        {
            //избавляемся от нежелательного поведения с прыжком && _rigidbody.velocity.y <= 0.1f при 0 могут
            // возникнуть проблемы с прыжком с пружинящей поверхности
            if (isGraunded() && _rigidbody.velocity.y <= 0.1f)
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
        _animator.SetBool("is-running", _direction.x != 0);
        // летим вверх или вниз
        _animator.SetFloat("vertical-velocity", _rigidbody.velocity.y);
        // на земле?
        _animator.SetBool("is-ground", isGraunded());
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    
    //пересекается ли объект с землей
    private bool isGraunded()
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
        //Debug.DrawRay(transform.position,Vector3.down, isGraunded() ? Color.green : Color.red);
        Gizmos.color = isGraunded() ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position + _groundCheckPoisitionDelta, _groundCheckRadius);
    }
    
    
}
