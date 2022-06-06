using System;
using Components.ColliderBased;
using Components.GoBased;
using UnityEngine;
using Utils;

namespace Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        // кидаться и кусаться
        
        [SerializeField] private LayerCheck _vision;
        
        [Header("Melee")]
        [SerializeField] private Cooldown _meleeCooldown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;

        [Header("Range")]
        [SerializeField] private Cooldown _rangeCooldown;
        [SerializeField] private SpawnComponent _rangeAttack;
        
        // несколько анимационных ивентов
        
        private static readonly int Melee = Animator.StringToHash("melee");
        private static readonly int Range = Animator.StringToHash("range");
        
        private Animator _animator;

        // получим аниматор
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            // если что то попадает в _vision и если мы можем атаковать
            if (_vision.isTouchingLayer)
            {
                if (_meleeCanAttack.isTouchingLayer)
                {
                    // и если кд готов
                    if (_meleeCooldown.IsReady)
                    {
                        MeleeAttack();
                        return; // чтобы не было дальней атаки вблизи
                    }
                }
                // если кд на дальную атаку готово
                if (_rangeCooldown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        private void RangeAttack()
        {
            _animator.SetTrigger(Range);
        }

        private void MeleeAttack()
        {
            _animator.SetTrigger(Melee);
        }
        
        // методы которые будем вызывать из анимационных эффектов
        // проверяем урон
        private void OnMeleeAttack()
        {
            _meleeCooldown.Reset();
            _meleeAttack.Check();
        }
        
        // кидание жемчужин
        private void OnRangeAttack()
        {
            _rangeCooldown.Reset();
            _rangeAttack.Spawn();
        }

    }
}