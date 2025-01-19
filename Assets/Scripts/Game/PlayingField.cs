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

        public GameObject Highlight => _highlight;
        public GameObject Base => _base;
        public BoxCollider PlayerBase => _playerBaseCollider;
        public Transform[] EnemySpawnPositions => _enemySpawnPositions;
        public Slider PlayerHealthSlider => _playerHealthSlider;
        public TextMeshProUGUI PlayerHealthAmountText => _playerHealthAmountText;
    }
}