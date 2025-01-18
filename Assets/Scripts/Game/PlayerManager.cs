using UnityEngine;

namespace SnakeWorks
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }

        public int Health { get; private set; } = 100;

        void Awake()
        {
            Instance = this;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                GameManager.Instance.SetState(GameState.Dead);
            }
        }
    }
}