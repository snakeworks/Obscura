using System.Collections.Generic;
using UnityEngine;

namespace SnakeWorks
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _actionPoint;
        [SerializeField] private int _damage = 10;
        [SerializeField] private float _bulletSpeed = 10.0f;
        [SerializeField] private float _shootDelay = 0.5f;

        private float _shootTimer = 0.0f;
        private List<EnemyBody> _enemiesInRange = new();

        void Update()
        {
            _shootTimer += Time.deltaTime;
            if (_shootTimer >= _shootDelay)
            {
                _shootTimer = 0.0f;
                Shoot();
            }
        }

        void Shoot()
        {
            // TODO: Lol
            List<EnemyBody> shootables = new();
            foreach (var e in _enemiesInRange)
            {
                if (e != null)
                {
                    shootables.Add(e);
                }
            }
            if (shootables.Count <= 0)
            {
                return;
            }
            var targetPos = shootables[0].transform.position;
            var bullet = Instantiate(_bulletPrefab, _actionPoint);
            bullet.Init(targetPos, _damage, _bulletSpeed);
        }

        void OnTriggerEnter(Collider collider)
        {
            var enemyBody = collider.GetComponent<EnemyBody>();
            if (enemyBody)
            {
                _enemiesInRange.Add(enemyBody);
            }
        }
        void OnTriggerExit(Collider collider)
        {
            var enemyBody = collider.GetComponent<EnemyBody>();
            if (enemyBody)
            {
                _enemiesInRange.Remove(enemyBody);
            }
        }
    }
}
