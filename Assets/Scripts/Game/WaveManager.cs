using System.Collections;
using TMPro;
using UnityEngine;

namespace SnakeWorks
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private EnemyBody _enemyPrefab;
        [SerializeField] private TextMeshProUGUI _roundText;

        public int CurrentWave { get; private set; } = 0;
        public int EnemiesLeftCount { get; private set; }
        public int EnemiesToSpawnCount { get; private set; }
        public int EnemiesSpawnedCount { get; private set; }

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
            EnemiesToSpawnCount = CurrentWave * 5;
            EnemiesLeftCount = EnemiesToSpawnCount;
            EnemiesSpawnedCount = 0;
            StartCoroutine(SpawnEnemy());
        }

        IEnumerator SpawnEnemy()
        {
            if (EnemiesSpawnedCount >= EnemiesToSpawnCount)
            {
                yield break;
            }
            
            yield return new WaitForSeconds(0.8f);
            
            EnemiesSpawnedCount += 1;

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

            StartCoroutine(SpawnEnemy());
        }

        void OnEnemyDied()
        {
            EnemiesLeftCount -= 1;
            if (EnemiesLeftCount <= 0)
            {
                NextWave();
            }
        }
    }
}
