﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Components
{
    public class SpawnListComponent : MonoBehaviour
    // будет агрегировать несколько спаун компонентов, которые будут вызываться по имени
    {
        // массив специальных классов
        [SerializeField] private SpawnData[] _spawners;

        public void Spawn(string id)
        {
            // foreach (var data in _spawners)
            // {
            //     if (data.Id == id)
            //     {
            //         data.Component.Spawn();
            //         break;
            //     }
            // }
            
            // или главное чтобы не откручивался в Update создает нагрузку
            var spawner = _spawners.FirstOrDefault(element => element.Id == id);
            spawner?.Component.Spawn();
        }
        
        [Serializable]
        public class SpawnData
        {
            public string Id;
            public SpawnComponent Component;
        }
    }
}