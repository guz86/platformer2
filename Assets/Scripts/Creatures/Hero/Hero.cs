using System;
using System.Collections;
using Components;
using Components.ColliderBased;
using Components.Health;
using Model.Data;
using UnityEditor.Animations;
using UnityEngine;
using Utils;

namespace Creatures.Hero
{
   // public class Hero : Creature
   // класс будет реализовывать дополнительно интерфейс
   public class Hero : Creature, ICanAddInInventory
    {
    
        // Ground указываем для поверхностей от которых можно прыгать, на поверхности вешаем слой Ground
        // перевод на ColliderCheck
        // [SerializeField] private float _groundCheckRadius;
        // перевод на ColliderCheck
        //[SerializeField] private Vector3 _groundCheckPoisitionDelta;
    
   

        // радиус действия для использования переключателя
        //[SerializeField] private float _interactiveRadius;

        // вместо интеракций будет использоваться CheckCircleOverlap
        // маска на переключателе
        //[SerializeField] private LayerMask _interectiveLayer;

        // задаем скорость для проигрывания анимации приземления
        [SerializeField] private float _slamDownVelocity;

        // для кулдауна кидания мечей
        [SerializeField] private Cooldown _throwCooldown;

        [Space]
        //[Header("Particles")]
        // чтобы не вызывать каждый компонент по отдельности выделим SpawnListComponent
        // для анимации пыли при беге перенесли в базовый SpawnListComponent
        //[SerializeField] private SpawnComponent _footStepPosition;

        // для партиклов при нанесении урона, разлетающиеся монетки
        //[SerializeField] private ParticleSystem _hitParticles; // берем реальные монеты как из бочки
        [SerializeField] private ProbabilityDropComponent _hitDrop;
    
        //триггер для анимации бросания меча
        private static readonly int ThrowKey = Animator.StringToHash("throw");
        
        //анимация для цепляние за стену
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");
    
        // для партиклов при прыжке перенесли в базовый SpawnListComponent
        //[SerializeField] private SpawnComponent _jumpPaticles;

        // для партиклов при падении перенесли в базовый SpawnListComponent
        //[SerializeField] private SpawnComponent _slamDownParticle;

        // в качестве параметров указать соотвествующие аниматоры
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        // настройки для кидания комбо мечей 3шт
        [Header("Super throw")]
        [SerializeField] private Cooldown _SuperThrowCooldown;
        [SerializeField] private int _superThrowParticles; // количество выкидываемых мечей
        [SerializeField] private float _superThrowDelay; // сколько секунд пройдет между броском
        
        // для механики цепляния к стенам
        [SerializeField] private ColliderCheck _wallCheck;
    
        private bool _isOnWall;
        private float _defaultGravityScale;
        
        private bool _superThrow;
    
        // вместо интеракций будет использоваться CheckCircleOverlap
        // массив объектов из 1 элемента, использование переключателя
        //private Collider2D[] _interactiveResult = new Collider2D[1];
        [SerializeField] private CheckCircleOverlap _intarationCheck;

        // можно ли делать двойной прыжок
        private bool _allowDoubleJump;

        //private int _coins;

        // для того чтобы герой смог поднимать оружие
        //private bool _isArmed;

        // для сохранения данных в сессии
        private GameSession _session;

        //для использования зелья
        private HealthComponent _health;

        // считаем мечи>монетки
        private int SwordCount => _session.Data.Inventory.Count("Sword");
        private int CoinCount => _session.Data.Inventory.Count("Coin");

        protected override void Awake()
        {
            base.Awake();
            // было для разваорота героя _sprite = GetComponent<SpriteRenderer>();
            // для цепляния к стенам
            _defaultGravityScale = Rigidbody.gravityScale;
        }

        // для сохранения данных в сессии
        private void Start()
        {
            // берем данные с текущей сессии, после очистки в class GameSession в Awake 
            _session = FindObjectOfType<GameSession>(); 
            // обновляем состояние с оружием, вооружаем
            UpdateHeroWeapon();
            // инициализация здоровья на герое
            _health = GetComponent<HealthComponent>();
            // метод из HealthComponent в него передаем состояние hp в начале
            _health.SetHealth(_session.Data.Hp);
            
            //для отслеживания поднятия предметов, метод-подписчик OnInventoryChanged
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
        }

        private void OnDestroy()
        {
            // обязательно отписаться
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)
        {
            // подбор оружия
            if (id == "Sword")
            {
                UpdateHeroWeapon();
            }
        }
        

        // для обновления здоровья на объекте в _OnChange в HealthComponent
        // добавим этот метод OnHealthChange
        public void OnHealthChange(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }

        protected override void Update()
        {
            base.Update();
            //для цепляния к стенам
            // нужно понять что мы давим в тоже самое направление куда и смотрит спрайт
            var moveToSomeDirection = Direction.x * transform.lossyScale.x > 0;
            // если больше нуля, то мы давим в ту же сторону куда развернулся герой
            
            // если соприкосается и еще давит на стену, то без гравитации и вешаем bool
            // мы будем висеть на стене если двигаемся в ее сторону
            if (_wallCheck.isTouchingLayer && moveToSomeDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
                
                // чтобы не ездил по стенам вверх
                Direction.y = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
            
            // для проигрывания анимации цепляния за стену
            Animator.SetBool(IsOnWall,_isOnWall);
        }
  
        protected override float CalculateYVelocity()
        {
        
            // следим за тем нажали ли мы кнопку вверх, изменилась ли координата y
            bool isJumpingPressing = Direction.y > 0;

            //если стоим на земле или висим на стене, то можем сделать двойной прыжок
            if (IsGrounded || _isOnWall) _allowDoubleJump = true;
        
            // если он не прыгает и висит на стене то мы не двигаемся по вертикальной координате
            if (!isJumpingPressing && _isOnWall)
            {
                return 0f;
            }
        
            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            // расчет скорости прыжка
            // если мы не на земле и у нас уже доступен двойной прыжок
            // if (!IsGrounded && _allowDoubleJump)
            // добавим !_isOnWall чтобы нельзя было сразу же прыгать со стены,
            // только после того как отпустимся от стены
            if (!IsGrounded && _allowDoubleJump && !_isOnWall)
            {
                // анимация прыжка
                Particles.Spawn("Jump");
                
                //звук прыжка
                DoJumpVfx();
                
                // запретим прыгать 2й раз
                _allowDoubleJump = false;
                return JumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }


        // //пересекается ли объект с землей
        // private bool IsGrounded()
        // {
        //     //луч
        //     //var hit = Physics2D.Raycast(transform.position, Vector2.down,1,_groundLayer);
        //     //сфера
        //     var hit = Physics2D.CircleCast(transform.position + _groundCheckPoisitionDelta, _groundCheckRadius,
        //         Vector2.down, 0, _groundLayer);
        //     return hit.collider != null;
        // }

        // при компиляции на другие платформы - чтобы код не вырезался, Handles нет в других сборках
/*#if UNITY_EDITOR
    //отслеживание пересечения с землей
    private void OnDrawGizmos()
    {
        //Debug.DrawRay(transform.position,Vector3.down, isGrounded() ? Color.green : Color.red);
        // через Gizmos
        //Gizmos.color = _isGrounded ? Color.green : Color.red;
        //Gizmos.DrawSphere(transform.position + _groundCheckPoisitionDelta, _groundCheckRadius);


        // другой вариант
        Handles.color = _isGrounded ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
        // диск в центом в transform.position, направление Vector3.forward, 
        Handles.DrawSolidDisc(transform.position + _groundCheckPoisitionDelta,
            Vector3.forward,
            _groundCheckRadius);
    }
#endif*/



        // добавление чего угодно
        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }
        
        // // добавление монеток, убираем с добавлением инвентаря
        // public void AddCoins(int coins)
        // {
        //     _session.Data.Coins += coins;
        //     Debug.Log($"Монеток: {_session.Data.Coins}");
        // }
        
        

        // получение урона от пик, вызов анимации, подброс героя вверх
        public override void TakeDamage()
        {
            base.TakeDamage();

            // ограничение вылетающих монет при причинении урона герою
            if (CoinCount > 0)
            {
                SpawnCoins();
            }
        }

        // разлетающиеся монетки от урона
        private void SpawnCoins()
        { 
            // считаем количество монет, либо 5 либо меньше, из того что есть у героя
            var numCoinsToDispose = Mathf.Min(CoinCount, 5);
            // убираем вылетающие монеты
            //_session.Data.Coins -= numCoinsToDispose;
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);
            
            //сделаем отдельный метод, в который будем передавать количество монет
            _hitDrop.SetCount(numCoinsToDispose);
            _hitDrop.CalculateDrop(); // в самом дропе будет выборос только сильверкойнов
            
            
            // // устаревший выброс пустых монет
            // // получаем текущую настройку для вылета партиколов
            // var burst = _hitParticles.emission.GetBurst(0);
            // // передаем ей количество койнов для вылета
            // burst.count = numCoinsToDispose;
            // // сохраним новое значение
            // _hitParticles.emission.SetBurst(0, burst);
            //
            // _hitParticles.gameObject.SetActive(true);
            //
            // _hitParticles.Play();
        }

        // после нажатия пробела, проверяем пересечения героя с объектом(переключателем)
        // если есть пересечение получим компонент InteractableComponent объекта и вызовем метод Interact
        public void Interact()
        {
            _intarationCheck.Check();
            /*var size = Physics2D.OverlapCircleNonAlloc(transform.position,
            // радиус
            _interactiveRadius,
            // массив объектов из 1 элемента
            _interactiveResult,
            // маска
            _interectiveLayer);
        // размер результатов size, пройдемся по массиву
        for (int i = 0; i < size; i++)
        {
            // из массива получим компонент объекта и вызывем метод из него
            var interactable = _interactiveResult[i].GetComponent<InteractableComponent>();
            interactable.Interact();
        }*/
        }

        //  будем вызывать напрямую из аниматора 
        //  для добавление анимации пыли в таймлайн анимации run на герое
        // public void SpawnFootDust()
        // {
        //     _footStepPosition.Spawn();
        // }

        // // заберем партиклы от объекта для TeleportComponent // убираем т.к. стали выбрасывать реальные
        // public ParticleSystem GetHitParticleSystem()
        // {
        //     return _hitParticles;
        // }

        // для анимации приземления
        // нам нужно понять что мы приземлились с определенной скоростью
        private void OnCollisionEnter2D(Collision2D other)
        {
            // через метод расширения, если мы соприкоснулись с землей
            if (other.gameObject.IsInLayer(GroundLayer))
            {
                var contract = other.contacts[0];
                // скорость между 2 колладеров,
                // если скорость больше определенного значения то анимируем падение
                if (contract.relativeVelocity.y >= _slamDownVelocity)
                {
                    Particles.Spawn("SlamDown");
                    //_slamDownParticle.Spawn();
                }
            }
        }

        
        public override void Attack()
        {
             
            
            // если не вооружен пропустить
            //if (!_session.Data.IsArmed)
            if (SwordCount <= 0)
            {
                return;
            }

            base.Attack();
        }

        /*public void OnAttackKey()
    {
        // достаем наши объекты с которые атакуем
        var gos = _attackRange.GetObjectsInRange();
        // попробуем у них получить компонетнт здоровья
        foreach (var go in gos)
        {
            var hp = go.GetComponent<HealthComponent>();
            if (hp != null)
            {
                // если есть здоровье отнимаем значение урона от оружия
                hp.ModifyHealth(-_damage);
            }
        }
    }*/

        // убираем после добавления инвентаря где идет подсчет мечей
        // public void ArmHero()
        // {
        //     // вооружаем героя
        //     _session.Data.IsArmed = true;
        //     UpdateHeroWeapon();
        // }

        // вооружаем
        // обновим данные героя на старте для оружия
        private void UpdateHeroWeapon()
        {
            // если вооружен/невооружен в сессии, то анимация
            //if (_session.Data.IsArmed)
            if (SwordCount > 0)
            {
                Animator.runtimeAnimatorController = _armed;
            }
            else
            {
                Animator.runtimeAnimatorController = _disarmed;
            }
        }

        
        // в SpawnListComponent добавляем объект TrowSwordSpamPosition с анимацией летящего меча
        // подвяжем вылет 
        public void OnDoThrow()
        {
            // время для броска, проверяем это супербросок
            if (_superThrow)
            {
                //рассчитаем количество мечей
                var numThrows = Mathf.Min(_superThrowParticles, SwordCount - 1);
                StartCoroutine(DoSuperThrow(numThrows));
            }
            else
            {
                ThrowRemoveFromInventory();
            }

            _superThrow = false;
        }

        private IEnumerator DoSuperThrow(int numThrows)
        {
            // для того чтобы добавить ожидание используем StartCoroutine
            for (int i = 0; i < numThrows; i++)
            {
                ThrowRemoveFromInventory();
                yield return new WaitForSeconds(_superThrowDelay);
            }
        }

        // выносим обычный бросок
        private void ThrowRemoveFromInventory()
        {
            
            //звук броска меча
            Sounds.Play("Throw");
            
            Particles.Spawn("Throw");
            
            // заберу один меч Sword
            _session.Data.Inventory.Remove("Sword", 1);
        }
    
        // убирем, вместо него PerformThrowing()
        // public void Throw()
        // {
        //     // если кулдаун прошел, то мы стартуем анимацию и сбрасываем время кулдауна
        //     if (_throwCooldown.IsReady && SwordCount > 1)
        //     {
        //         Animator.SetTrigger(ThrowKey);
        //         _throwCooldown.Reset();
        //     }
        // }

        public void StartThrowing()
        {
            // зажали кнопку и нужно начать отчитывать кулдаун
            _SuperThrowCooldown.Reset();
            
        }

        public void PerformThrowing()
        {
            // отпустили кнопку
            // если не готовы к броску, то выйдем
            if (!_throwCooldown.IsReady ||  SwordCount <= 1) return;
            
            // если готов супербросок, поднимаем флаг
            if (_SuperThrowCooldown.IsReady)
            {
                _superThrow = true;
            }
            // сам бросок будем совершать на OnDoThrow()
            
            //стартуем анимацию и сбрасываем время кулдауна
            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
        }

        // использование зелья здоровья
        public void UsePotion()
        {
            var potionCount = _session.Data.Inventory.Count("HealthPotion");
            if (potionCount > 0)
            {
                // добавляем 5 здоровья, убираем 1 зелье из инвентаря
                _health.ModifyHealth(5);
                _session.Data.Inventory.Remove("HealthPotion", 1);
            }
        }
    }
}