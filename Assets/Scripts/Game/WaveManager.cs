using System.Collections;
using TMPro;
using UnityEngine;

namespace SnakeWorks
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private EnemyBody _enemyPrefab;
        [SerializeField] private TextMeshProUGUI _roundText;
        [SerializeField] private TextMeshProUGUI _gameOverRoundText;

        public int CurrentWave { get; private set; } = 0;
        public int EnemiesLeftCount { get; private set; }
        public int EnemiesToSpawnCount { get; private set; }

        private TextMeshProUGUI _enemyCountText => GameManager.Instance.PlayingField.EnemyCountText;

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
            _roundText.SetText(CurrentWave.ToString());
            _gameOverRoundText.SetText(_roundText.text);
            EnemiesToSpawnCount = CurrentWave * 5;
            EnemiesLeftCount = EnemiesToSpawnCount;
            UpdateEnemyCountUI();
            StartCoroutine(SpawnEnemy());
        }

        IEnumerator SpawnEnemy()
        {
            for (int i = 0; i < EnemiesToSpawnCount; i++)
            {
                yield return new WaitForSeconds(0.8f);

                var posLength = GameManager.Instance.PlayingField.EnemySpawnPositions.Length;
                var randPosIndex = Random.Range(0, posLength);
                var randPos = GameManager.Instance.PlayingField.EnemySpawnPositions[randPosIndex];
                
                var enemyBody = Instantiate(
                    _enemyPrefab,
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
