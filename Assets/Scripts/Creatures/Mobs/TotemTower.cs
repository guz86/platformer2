using System.Collections.Generic;
using System.Linq;
using Components.Health;
using UnityEngine;
using Utils;

namespace Creatures.Mobs
{
    public class TotemTower : MonoBehaviour
    {
        [SerializeField] private List<ShootingTrapAI> _traps;
        [SerializeField] private Cooldown _cooldown;
        //текущая стреляющая пушка
        private int _currentTrap;

        private void Start()
        {
            // выключим ловушки
            foreach (var shootingTrapAI in _traps)
            {
                shootingTrapAI.enabled = false;
                // нужно понять что мы разрушили башню
                var hp = shootingTrapAI.GetComponent<HealthComponent>();
                hp._onDie.AddListener(() => OnTrapDead(shootingTrapAI)); // подписываемся на событие
                // _onDie юнитиивент пустой, создаем замыкание(анонимную функцию)  () =>
                // и в нее передаем OnTrapDead(shootingTrapAI))
            }
        }

        public void OnTrapDead(ShootingTrapAI shootingTrapAI)
        {
            // нужно так же сместить индекс, т.к. мы удляем объект из списка
            var index = _traps.IndexOf(shootingTrapAI); // текущий
            
            _traps.Remove(shootingTrapAI); // удаляем его из списка

            if (index < _currentTrap)
            {
                _currentTrap--;
            }
        }

        private void Update()
        {
            // в конец удаляем сам TotemTower 
            if (_traps.Count == 0)
            {
                enabled = false;
                Destroy(gameObject,1f); // удалим через 1 секунду чтобы успели заспаунить мусор
            }
            
            
            // видит ли какая-либо из ловушек героя
            var hasAnyTarget = _traps.Any(x => x._vision.isTouchingLayer);
            if (hasAnyTarget)
            {
                if (_cooldown.IsReady)
                {
                    _traps[_currentTrap].Shoot();
                    _cooldown.Reset();
                    // текущая стреляет и берем следующую
                    // прибавляем, если больше чем количество ловушек вернемся к нулю
                    _currentTrap = (int) Mathf.Repeat(_currentTrap+1, _traps.Count);
                } 
            }
        }
    }
}