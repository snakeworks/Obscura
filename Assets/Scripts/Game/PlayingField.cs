using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeWorks
{
    public class PlayingField : MonoBehaviour
    {
        [SerializeField] private GameObject _highlight;
        [SerializeField] private GameObject _base;
        [SerializeField] private BoxCollider _playerBaseCollider;
        [SerializeField] private Transform[] _enemySpawnPositions;
        [SerializeField] private Slider _playerHealthSlider;
        [SerializeField] private TextMeshProUGUI _playerHealthAmountText;
        [SerializeField] private TextMeshProUGUI _enemyCountText;
        [SerializeField] private Transform _turretsParent;

        public GameObject Highlight => _highlight;
        public GameObject Base => _base;
        public BoxCollider PlayerBase => _playerBaseCollider;
        public Transform[] EnemySpawnPositions => _enemySpawnPositions;
        public Slider PlayerHealthSlider => _playerHealthSlider;
        public TextMeshProUGUI PlayerHealthAmountText => _playerHealthAmountText;
        public TextMeshProUGUI EnemyCountText => _enemyCountText;

        private ShopItem _turretItem;

        void Awake()
        {
            foreach (Transform child in _turretsParent)
            {
                child.GetComponent<Turret>().enabled = false;
                child.gameObject.SetActive(false);
            }
        }

        void Start()
        {
            _turretItem = ShopManager.Instance.GetItem("turret");
            _turretItem.Purchased += OnTurretPurchased;
        }

        void OnTurretPurchased()
        {
            var turretObj = _turretsParent.GetChild(_turretItem.Amount-1).gameObject;
            turretObj.GetComponent<Turret>().enabled = true;
            turretObj.SetActive(true);
        }
    }
}