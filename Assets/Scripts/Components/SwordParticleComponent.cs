using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordParticleComponent : MonoBehaviour
{
    
    // для анимации подбора меча
    [SerializeField] private ParticleSystem _swordParticles;
    
    // для анимации подъема меча
    public void SpawnSwordParticles()
    {
        Instantiate(_swordParticles, transform.position, Quaternion.identity);
    }
}
