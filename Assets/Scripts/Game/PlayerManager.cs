using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeWorks
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }

        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private int _damage = 1;
        [SerializeField] private int _points = 0;
        [SerializeField] private TextMeshProUGUI _pointsText;
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private LayerMask _enemyLayer;
        
        private Slider _healthSlider => GameManager.Instance.PlayingField.PlayerHealthSlider;
        private TextMeshProUGUI _healthText => GameManager.Instance.PlayingField.PlayerHealthAmountText;

        private Vector2 _lastScreenPosition;

        void Awake()
        {
            Instance = this;
            _health = _maxHealth;
        }

        void Start()
        {
            GameManager.Instance.GameStateChanged += OnGameStateChanged;
        }

        void OnGameStateChanged()
        {
            if (GameManager.Instance.CurrentGamestate == GameState.Playing)
            {
                UpdateHealthUI();
            }
            else if (GameManager.Instance.CurrentGamestate == GameState.Dead)
            {
                UIManager.OpenScreen(_gameOverScreen);
            }
        }

        public void TakeDamage(int damage)
        {
            if (GameManager.Instance.CurrentGamestate != GameState.Playing)
            {
                return;
            }

            _health -= damage;
            UpdateHealthUI();
            if (_health <= 0)
            {
                GameManager.Instance.SetState(GameState.Dead);
            }
        }

        public void AddPoints(int value)
        {
            _points += value;
            _pointsText.SetText(_points.ToString("N0"));
        }

        void Update()
        {
            if (GameManager.Instance.CurrentGamestate != GameState.Playing)
            {
                return;
            }

            var screenRaycast = GameManager.Instance.TapRaycast.ReadValue();
            if (screenRaycast != Vector2.zero)
            {
                _lastScreenPosition = screenRaycast;
            }

            if (GameManager.Instance.TapAction.ReadWasPerformedThisFrame())
            {
                var hit = ARObjectManager.Instance.GetRaycastHit(_lastScreenPosition, _enemyLayer);
                if (hit)
                {
                    hit.GetComponent<EnemyBody>().TakeDamage(_damage);
                }
            }
        }

        void UpdateHealthUI()
        {
            _healthSlider.maxValue = _maxHealth;
            _healthSlider.value = _health;
            _healthText.SetText($"{_health}/{_maxHealth}");
        }
    }
}