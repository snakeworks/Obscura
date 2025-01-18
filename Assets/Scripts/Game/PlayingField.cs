using UnityEngine;

namespace SnakeWorks
{
    public class PlayingField : MonoBehaviour
    {
        [SerializeField] private GameObject _highlight;
        [SerializeField] private BoxCollider _playerBase;
        [SerializeField] private Transform[] _enemySpawnPositions;

        public GameObject Highlight => _highlight;
        public BoxCollider PlayerBase => _playerBase;
        public Transform[] EnemySpawnPositions => _enemySpawnPositions;
    }
}