using System;
using DG.Tweening;
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
        [SerializeField] private int _points = 0;
        [SerializeField] private TextMeshProUGUI _pointsText;
        [SerializeField] private TextMeshProUGUI _pointsFloatingTextPrefab;
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private LayerMask _enemyLayer;
        
        public int Points => _points;

        private int Damage
        {
            get
            {
                var amount = ShopManager.Instance.GetItemAmount("tap_damage");
                return amount > 0 ? amount+1 : 1;
            }
        }

        private Slider _healthSlider => GameManager.Instance.PlayingField.PlayerHealthSlider;
        private TextMeshProUGUI _healthText => GameManager.Instance.PlayingField.PlayerHealthAmountText;

        private Vector2 _lastScreenPosition;

        public event Action PointsChanged;

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
            PointsChanged?.Invoke();

            TextMeshProUGUI floatingText = Instantiate(_pointsFloatingTextPrefab, _pointsText.transform);
            floatingText.SetText($"{(value < 0 ? "" : "+")}{value:N0}");
            if (value < 0)
            {
                floatingText.color = Color.red;
            }
            
            floatingText.transform.localPosition = new Vector3(
                floatingText.transform.localPosition.x + 60,
                floatingText.transform.localPosition.y,
                floatingText.transform.localPosition.z
            );
            floatingText.transform.DOLocalMoveY(floatingText.transform.localPosition.y + 50, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Destroy(floatingText.gameObject);
            });
            floatingText.DOFade(0, 0.25f).SetDelay(0.25f);
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
                    hit.GetComponent<EnemyBody>().TakeDamage(Damage);
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