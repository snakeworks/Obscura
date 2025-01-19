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

        void Awake()
        {
            _welcomeScreen.SetActive(false);
            _placeFieldScreen.SetActive(false);
            _gameScreen.SetActive(false);
        }

        void Start()
        {
            UpdateUI();

            _deleteButton.onClick.AddListener(DeleteField);
            _startButton.onClick.AddListener(StartGame);
        }

        void DeleteField()
        {
            if (GameManager.Instance.PlayingField != null)
            {
                Destroy(GameManager.Instance.PlayingField.gameObject);
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
                        && GameManager.Instance.PlayingField == null)
                    {
                        GameManager.Instance.PlayingField = ARObjectManager.Instance.TrySpawnObjectRaycast(_playingField)?.GetComponent<PlayingField>();
                        if (GameManager.Instance.PlayingField != null)
                        {
                            GameManager.Instance.PlayingField.Highlight.SetActive(true);
                            GameManager.Instance.PlayingField.Base.SetActive(false);
                        }
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
                    UIManager.OpenScreen(_welcomeScreen);
                    break;
                case GameState.PlacingPlayingField:
                    if (GameManager.Instance.PlayingField != null)
                    {
                        _deleteButton.gameObject.SetActive(true);
                        _startButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        _deleteButton.gameObject.SetActive(false);
                        _startButton.gameObject.SetActive(false);
                    }
                    UIManager.OpenScreen(_placeFieldScreen);
                    break;
                case GameState.Playing:
                    UIManager.OpenScreen(_gameScreen);
                    break;
            }
        }
    }
}
