using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace SnakeWorks
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] public XRInputButtonReader TapAction;
        [SerializeField] public XRInputValueReader<Vector2> TapRaycast;
        [SerializeField] private ARPlaneManager _arPlaneManager;

        public GameState CurrentGamestate { get; private set; } = GameState.Welcome;
        public PlayingField PlayingField { get; set; }

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
            if (state == GameState.Playing)
            {
                foreach (var trackable in _arPlaneManager.trackables)
                {
                    trackable.gameObject.GetComponent<MeshRenderer>().materials = Array.Empty<Material>();
                }
                _arPlaneManager.enabled = false;
                PlayingField.Base.SetActive(true);
                PlayingField.Highlight.SetActive(false);
                PlayingField.GetComponent<XRGrabInteractable>().enabled = false;
            }
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
