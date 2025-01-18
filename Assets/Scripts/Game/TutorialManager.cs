using UnityEngine;
using UnityEngine.UI;

namespace SnakeWorks
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private GameObject _welcomeScreen;
        [SerializeField] private GameObject _placeFieldScreen;
        [SerializeField] private GameObject _gameScreen;
        [SerializeField] private Button _deleteButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private GameObject _playingField;

        void Start()
        {
            UpdateUI();

            _deleteButton.onClick.AddListener(DeleteField);
            _startButton.onClick.AddListener(StartGame);
        }

        void DeleteField()
        {
            if (ARObjectManager.Instance.SpawnedObjects.Count > 0)
            {
                ARObjectManager.Instance.DestroyObject(GameManager.Instance.PlayingField.gameObject);
            }
        }

        void StartGame()
        {
            GameManager.Instance.SetState(GameState.Playing);
            UpdateUI();
            gameObject.SetActive(false);
            enabled = false;
        }

        void Update()
        {
            switch (GameManager.Instance.CurrentGamestate)
            {
                case GameState.Welcome:
                    if (GameManager.Instance.TapAction.ReadWasPerformedThisFrame())
                    {
                        GameManager.Instance.SetState(GameState.PlacingPlayingField);
                    }
                    break;
                case GameState.PlacingPlayingField:
                    if (GameManager.Instance.TapAction.ReadWasPerformedThisFrame() 
                        && ARObjectManager.Instance.SpawnedObjects.Count <= 0)
                    {
                        ARObjectManager.Instance.TrySpawnObjectRaycast(_playingField);
                    }
                    break;
            }
            UpdateUI();
        }

        void UpdateUI()
        {
            switch (GameManager.Instance.CurrentGamestate)
            {
                case GameState.Welcome:
                    _welcomeScreen.SetActive(true);
                    _placeFieldScreen.SetActive(false);
                    _gameScreen.SetActive(false);
                    break;
                case GameState.PlacingPlayingField:
                    if (ARObjectManager.Instance.SpawnedObjects.Count > 0)
                    {
                        _deleteButton.gameObject.SetActive(true);
                        _startButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        _deleteButton.gameObject.SetActive(false);
                        _startButton.gameObject.SetActive(false);
                    }
                    _welcomeScreen.SetActive(false);
                    _placeFieldScreen.SetActive(true);
                    _gameScreen.SetActive(false);
                    break;
                case GameState.Playing:
                    _welcomeScreen.SetActive(false);
                    _placeFieldScreen.SetActive(false);
                    _gameScreen.SetActive(true);
                    break;
            }
        }
    }
}
