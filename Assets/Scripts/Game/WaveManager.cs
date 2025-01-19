using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SnakeWorks
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private EnemyBody[] _enemyPrefabs;
        [SerializeField] private TextMeshProUGUI _roundText;
        [SerializeField] private TextMeshProUGUI _gameOverRoundText;

        private List<EnemyBody> _spawnableEnemyPrefabs = new();

        public int CurrentWave { get; private set; } = 0;
        public int EnemiesLeftCount { get; private set; }
        public int EnemiesToSpawnCount { get; private set; }

        private TextMeshProUGUI _enemyCountText => GameManager.Instance.PlayingField.EnemyCountText;

        private WaitForSeconds _enemySpawnDelaySeconds;
        private float _enemySpawnDelay = 0.8f;

        void Start()
        {
            GameManager.Instance.GameStateChanged += OnGameStateChanged;
        }

        void OnGameStateChanged()
        {
            if (GameManager.Instance.CurrentGamestate == GameState.Playing)
            {
                NextWave();
            }
        }

        void NextWave()
        {
            CurrentWave += 1;
            _enemySpawnDelay -= 0.05f;
            _enemySpawnDelay = Mathf.Clamp(_enemySpawnDelay, 0.3f, 1.0f);
            _enemySpawnDelaySeconds = new(_enemySpawnDelay);

            _roundText.SetText(CurrentWave.ToString());
            _gameOverRoundText.SetText(_roundText.text);
            EnemiesToSpawnCount = CurrentWave * 5;
            EnemiesLeftCount = EnemiesToSpawnCount;
            UpdateEnemyCountUI();

            _spawnableEnemyPrefabs.Clear();
            foreach (var enemy in _enemyPrefabs)
            {
                if (enemy.StartingRound <= CurrentWave)
                {
                    _spawnableEnemyPrefabs.Add(enemy);
                }
            }

            StartCoroutine(SpawnEnemy());
        }

        IEnumerator SpawnEnemy()
        {
            for (int i = 0; i < EnemiesToSpawnCount; i++)
            {
                yield return _enemySpawnDelaySeconds;

                var posLength = GameManager.Instance.PlayingField.EnemySpawnPositions.Length;
                var randPosIndex = Random.Range(0, posLength);
                var randPos = GameManager.Instance.PlayingField.EnemySpawnPositions[randPosIndex];
                var randPrefab = Random.Range(0, _spawnableEnemyPrefabs.Count);     

                var enemyBody = Instantiate(
                    _spawnableEnemyPrefabs[randPrefab],
                    randPos.position,
                    randPos.rotation,
                    GameManager.Instance.PlayingField.transform
                ).GetComponent<EnemyBody>();

                enemyBody.transform.localPosition = randPos.transform.localPosition;
                enemyBody.Init(OnEnemyDied);
            }
        }

        void OnEnemyDied()
        {
            if (GameManager.Instance.CurrentGamestate != GameState.Playing)
            {
                return;
            }
            EnemiesLeftCount -= 1;
            UpdateEnemyCountUI();
            if (EnemiesLeftCount <= 0)
            {
                NextWave();
            }
        }

        void UpdateEnemyCountUI()
        {
            _enemyCountText.SetText($"{EnemiesLeftCount}/{EnemiesToSpawnCount}");
        }
    }
}
