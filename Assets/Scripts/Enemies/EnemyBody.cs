using System;
using System.Collections;
using UnityEngine;

namespace SnakeWorks
{
    public class EnemyBody : MonoBehaviour
    {
        [SerializeField] private int _health = 3;
        [SerializeField] private int _damage = 1;
        [SerializeField] private int _pointValue = 10;
        [SerializeField] private float _speed = 5.0f;
        [SerializeField] private int _startingRound = 1;
        [SerializeField] private Collider _collider;
        [SerializeField] private Material _flashMaterial;
        [SerializeField] private Shader _dissolveShader;

        public int Health => _health;
        public int Damage => _damage;
        public float Speed => _speed;
        public int StartingRound => _startingRound;

        private MeshRenderer[] _meshRenderers;
        private Material _enemyMat;
        private WaitForSeconds _flashSeconds = new(0.03f);

        private Action _onDeathAction;

        private bool _isDead = false;

        public void Init(Action onDeathAction)
        {
            _onDeathAction = onDeathAction;
            _meshRenderers = new MeshRenderer[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                _meshRenderers[i] = transform.GetChild(i).GetComponent<MeshRenderer>();
            }
            _enemyMat = _meshRenderers[0].material;
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
                PlayerManager.Instance.AddPoints(_pointValue);
                Die();
            }
            else
            {
                StartCoroutine(Flash());
            }
        }

        IEnumerator Flash()
        {
            SetMaterial(_flashMaterial);
            yield return _flashSeconds;
            SetMaterial(_enemyMat);
        }

        void Die()
        {
            _isDead = true;
            _onDeathAction?.Invoke();
            SetMaterial(_flashMaterial);
            float dissolveProgress = 0.25f;
            StartCoroutine(DeathAnim());
            IEnumerator DeathAnim()
            {
                SetMaterial(_flashMaterial, _dissolveShader);
                while (dissolveProgress < 1.0f)
                {
                    dissolveProgress += Time.deltaTime * 3.5f;
                    SetShaderFloat("_DissolveThreshold", dissolveProgress);
                    yield return null;
                }
                Destroy(gameObject);
            }
        }

        void SetMaterial(Material material, Shader shader = null)
        {
            foreach (var renderer in _meshRenderers)
            {
                renderer.material = material;
                if (shader != null)
                {
                    renderer.material.shader = shader;
                }
            }
        }
        void SetShaderFloat(string propertyName, float value)
        {
            foreach (var renderer in _meshRenderers)
            {
                renderer.material.SetFloat(propertyName, value);
            }
        }

        void FixedUpdate()
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