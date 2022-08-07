using UnityEngine;

public class SwordParticleComponent : MonoBehaviour
{
    
    // для анимации подбора меча
    [SerializeField] private ParticleSystem _swordParticles;
    
    public void SpawnSwordParticles()
    {
        // создаем партикл в точке где расположен меч
        Instantiate(_swordParticles, transform.position, Quaternion.identity);
    }
}
