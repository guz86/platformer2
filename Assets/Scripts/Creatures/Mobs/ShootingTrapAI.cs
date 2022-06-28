using System;
using Components.Animations;
using Components.ColliderBased;
using UnityEngine;
using Utils;

namespace Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        //[SerializeField] private ColliderCheck _vision; // нужен для TotemTower
        [SerializeField] public ColliderCheck _vision;
        [SerializeField] private Cooldown _cooldown;
        //чтобы запустить анимацию атаки
        [SerializeField] private SpriteAnimationClips _animation;

        private void Update()
        {
            if (_vision.isTouchingLayer && _cooldown.IsReady)
            {
                Shoot();
            }
        }

        //private void Shoot() // нужен для TotemTower
        public void Shoot()
        {
            _cooldown.Reset();
            _animation.SetClip("start-attack");
        }
    }
}