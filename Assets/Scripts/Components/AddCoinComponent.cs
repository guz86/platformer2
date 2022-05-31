using UnityEngine;

namespace Components
{
    public class AddCoinComponent : MonoBehaviour
    {
        [SerializeField] private int _numberCouins;
        private Hero _heroPlayer;

        private void Start()
        {
            _heroPlayer = FindObjectOfType<Hero>();
        }

        public void Add()
        {
            _heroPlayer.AddCoins(_numberCouins);
        }
    }
}