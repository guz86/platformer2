﻿using System;
using System.Collections;
using Components;
using UnityEngine;

namespace Creatures
{
    public class MobAI : MonoBehaviour
    {
        // мог видеть, мог кусаться, мог ходить

        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;
        // время ожидания после того как моб увидел
        [SerializeField] private float _alarmDelay = 1f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _missHeroCooldown = 0.5f;
        
        // переменная где будет запущенная в текущий момент корутина
        private Coroutine _current;
        
        private GameObject _target;
        private Creature _creature;
        private Animator _animator;
        private bool _isDead;
        private Patrol _patrol;
        
        // для анимации AgroToHero
        private SpawnListComponent _particles;
        
        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");
        
        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            // для OnDie
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start()
        {
    //        StartState(Patrolling());
                StartState(_patrol.DoPatrol());
        }

        // в Vision в EnterTriggerComp по таг Player мы передадим этот метод
        // от Sharky для реагирования
        public void OnHeroOnVision(GameObject go)
        {
            // если умерли, то выходим
            if (_isDead)
            {
                return;
            }
            
            // увидели героя
            _target = go;
            // идем к нему
            StartState(AgroToHero());

        }

        private IEnumerator AgroToHero()
        {
            // Exclamation добавим в spawners на sharky
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            // идем к герою
            StartState(GoToHero());
            
        }

        private IEnumerator GoToHero()
        {
            // идем пока он есть в зоне видимости
            while (_vision.isTouchingLayer)
            {
                //если в зоне действия атаки атакуем, иначе преследуем
                if (_canAttack.isTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }
                
                yield return null;
            }
            
            // если потеряли героя из виду
            _particles.Spawn("MissHero");
            yield return new WaitForSeconds(_missHeroCooldown);
            // сбрасываем направление движения если потеряли 
            _creature.SetDirection(Vector2.zero);
        }

        private IEnumerator Attack()
        {
            // атакуем пока есть возможность с кулдауном
            while (_canAttack.isTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            
            //если не можем атаковать, начинаем приследование
            StartState(GoToHero());
        }

        private void SetDirectionToTarget()
        {
            // двигаемся к герою выбираем направление движения(вектор направления)
            var direction = _target.transform.position - transform.position;
            // движемся только по горизонтали
            direction.y = 0;
            //_creature.SetDirection(direction);
            // чтобы не было перепадов по скорости normalized
            _creature.SetDirection(direction.normalized);
        }

        // private IEnumerator Patrolling()
        // {
        //     yield return new WaitForSeconds(1f);
        // }
        
        // Создадим для Sharky еще одну анимацию die и закинем ее в аниматор, из hit 
        // OnDie() вызовем его через HealthComponent
        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDeadKey,true);
            //останавливаем карутины
            if (_current != null)
            {
                StopCoroutine(_current);
            }
        }
        
        
        // система которая будет работать с карутинами
        // чтобы не возникло коллизий между двумя разными корутинами
        // моб должен делать только одну вещь за раз
        private void StartState(IEnumerator coroutine)
        {
            // сбрасываем направление движения при любом новом состоянии
            _creature.SetDirection(Vector2.zero);
            
            // если текущая запущена, то останавливаем
            if (_current != null)
            {
                StopCoroutine(_current);
            }
            // запускаем новую coroutine и передаем ее в текущую переменную
            _current = StartCoroutine(coroutine);
        }
    }
}