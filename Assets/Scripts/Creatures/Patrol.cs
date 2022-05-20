using System.Collections;
using UnityEngine;

namespace Creatures
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}