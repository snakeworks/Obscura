using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

namespace SnakeWorks
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] public XRInputButtonReader TapAction;

        public GameState CurrentGamestate { get; private set; } = GameState.Welcome;
        public PlayingField PlayingField => 
            ARObjectManager.Instance.SpawnedObjects.Count >= 0 
            ? ARObjectManager.Instance.SpawnedObjects[0].GetComponent<PlayingField>() 
            : null;

        public event Action GameStateChanged;

        void Awake()
        {
            Instance = this;
        }

        void OnEnable()
        {
            TapAction.EnableDirectActionIfModeUsed();
        }

        void OnDisable()
        {
            TapAction.DisableDirectActionIfModeUsed();
        }

        public void SetState(GameState state)
        {
            CurrentGamestate = state;
            GameStateChanged?.Invoke();
        }
    }
    
    public enum GameState : uint
    {
        Welcome = 0,
        PlacingPlayingField = 1,
        Playing = 2,
        Dead = 3
    }
}
