using System.Collections;
using UnityEngine;

namespace SnakeWorks
{
    public class Bullet : MonoBehaviour
    {
        private Vector3 _targetPosition;
        private int _damage;
        private float _speed;

        public void Init(Vector3 targetPosition, int damage, float speed)
        {
            _targetPosition = targetPosition;
            _damage = damage;
            _speed = speed;
            transform.LookAt(_targetPosition);
            StartCoroutine(Decay());
        }

        void FixedUpdate()
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.forward);
        }

        void OnTriggerEnter(Collider collider)
        {
            var enemyBody = collider.GetComponent<EnemyBody>();
            if (enemyBody)
            {
                enemyBody.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }

        IEnumerator Decay()
        {
            yield return new WaitForSeconds(3.0f);
            Destroy(gameObject);
        }
    }
}