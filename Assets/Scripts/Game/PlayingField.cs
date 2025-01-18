using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace SnakeWorks
{
    public class PlayingField : MonoBehaviour
    {
        [SerializeField] private GameObject _highlight;
        [SerializeField] private BoxCollider _playerBase;
        [SerializeField] private Transform[] _enemySpawnPositions;

        public BoxCollider PlayerBase => _playerBase;
        public Transform[] EnemySpawnPositions => _enemySpawnPositions;

        void Start()
        {
            GameManager.Instance.GameStateChanged += OnGameStateChanged;
        }

        void OnDestroy()
        {
            GameManager.Instance.GameStateChanged -= OnGameStateChanged;
        }
        
        void OnGameStateChanged()
        {
            if (GameManager.Instance.CurrentGamestate == GameState.Playing)
            {
                _highlight.SetActive(false);
                GetComponent<XRGrabInteractable>().enabled = false;
            }
        }
    }
}