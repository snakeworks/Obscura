using System;
using System.Collections;
using UnityEngine;

namespace SnakeWorks
{
    public class EnemyBody : MonoBehaviour
    {
        [SerializeField] private int _health = 3;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _speed = 5.0f;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material _flashMaterial;

        public int Health => _health;
        public int Damage => _damage;
        public float Speed => _speed;

        private Material _enemyMat;
        private WaitForSeconds _flashSeconds = new(0.03f);

        private Action _onDeathAction;



        public void Init(Action onDeathAction)
        {
            _onDeathAction = onDeathAction;
            _enemyMat = _meshRenderer.material;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(Flash());
            }
        }

        IEnumerator Flash()
        {
            _meshRenderer.material = _flashMaterial;
            yield return _flashSeconds;
            _meshRenderer.material = _enemyMat;
        }

        void Die()
        {
            _onDeathAction?.Invoke();
            Destroy(gameObject);
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