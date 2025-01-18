using System;
using UnityEngine;

namespace SnakeWorks
{
    public class EnemyBody : MonoBehaviour
    {
        [SerializeField] private int _health = 3;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _speed = 5.0f;

        public int Health => _health;
        public int Damage => _damage;
        public float Speed => _speed;

        private Action _onDeathAction;

        public void Init(Action onDeathAction)
        {
            _onDeathAction = onDeathAction;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                DestroyEnemy();
            }
        }

        void Update()
        {
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                transform.localPosition.y,
                transform.localPosition.z + _speed * Time.deltaTime
            );
        }
        
        void DestroyEnemy()
        {
            Destroy(gameObject);
            _onDeathAction?.Invoke();
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider == GameManager.Instance.PlayingField.PlayerBase)
            {
                PlayerManager.Instance.TakeDamage(_damage);
                DestroyEnemy();
            }
        }
    }
}