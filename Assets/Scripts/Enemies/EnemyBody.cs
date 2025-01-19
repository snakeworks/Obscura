using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace SnakeWorks
{
    public class EnemyBody : MonoBehaviour
    {
        [SerializeField] private int _health = 3;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _speed = 5.0f;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private Material _flashMaterial;
        [SerializeField] private Shader _dissolveShader;

        public int Health => _health;
        public int Damage => _damage;
        public float Speed => _speed;

        private Material _enemyMat;
        private WaitForSeconds _flashSeconds = new(0.03f);

        private Action _onDeathAction;

        private bool _isDead = false;

        public void Init(Action onDeathAction)
        {
            _onDeathAction = onDeathAction;
            _enemyMat = _meshRenderer.material;
        }

        public void TakeDamage(int damage)
        {
            if (_isDead)
            {
                return;
            }

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
            _isDead = true;
            _onDeathAction?.Invoke();
            _meshRenderer.material = _flashMaterial;
            float dissolveProgress = 0.25f;
            StartCoroutine(DeathAnim());
            IEnumerator DeathAnim()
            {
                _meshRenderer.material.shader = _dissolveShader;
                while (dissolveProgress < 1.0f)
                {
                    dissolveProgress += Time.deltaTime * 3.5f;
                    _meshRenderer.material.SetFloat("_DissolveThreshold", dissolveProgress);
                    yield return null;
                }
                Destroy(gameObject);
            }
        }

        void Update()
        {
            if (_isDead)
            {
                return;
            }
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                transform.localPosition.y,
                transform.localPosition.z + _speed * Time.deltaTime
            );
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider == GameManager.Instance.PlayingField.PlayerBase)
            {
                PlayerManager.Instance.TakeDamage(_damage);
                Die();
            }
        }
    }
}