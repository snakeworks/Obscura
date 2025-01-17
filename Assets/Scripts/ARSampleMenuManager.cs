using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.AR.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets
{
    /// <summary>
    /// Handles dismissing the object menu when clicking out the UI bounds, and showing the
    /// menu again when the create menu button is clicked after dismissal. Manages object deletion in the AR demo scene,
    /// and also handles the toggling between the object creation menu button and the delete button.
    /// </summary>
    public class ARSampleMenuManager : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Button that deletes a selected object.")]
        Button m_DeleteButton;

        /// <summary>
        /// Button that deletes a selected object.
        /// </summary>
        public Button deleteButton
        {
            get => m_DeleteButton;
            set => m_DeleteButton = value;
        }

        [SerializeField]
        [Tooltip("The object spawner component in charge of spawning new objects.")]
        ObjectSpawner m_ObjectSpawner;

        /// <summary>
        /// The object spawner component in charge of spawning new objects.
        /// </summary>
        public ObjectSpawner objectSpawner
        {
            get => m_ObjectSpawner;
            set => m_ObjectSpawner = value;
        }

        [SerializeField] private ARPlaneManager _planeManager;

        private bool _showDebugPlane = false;
        private Material[] _planeMaterials;

        [SerializeField]
        [Tooltip("The interaction group for the AR demo scene.")]
        XRInteractionGroup m_InteractionGroup;

        /// <summary>
        /// The interaction group for the AR demo scene.
        /// </summary>
        public XRInteractionGroup interactionGroup
        {
            get => m_InteractionGroup;
            set => m_InteractionGroup = value;
        }

        [SerializeField]
        XRInputValueReader<Vector2> m_TapStartPositionInput = new XRInputValueReader<Vector2>("Tap Start Position");

        /// <summary>
        /// Input to use for the screen tap start position.
        /// </summary>
        /// <seealso cref="TouchscreenGestureInputController.tapStartPosition"/>
        public XRInputValueReader<Vector2> tapStartPositionInput
        {
            get => m_TapStartPositionInput;
            set => XRInputReaderUtility.SetInputProperty(ref m_TapStartPositionInput, value, this);
        }

        void OnEnable()
        {
            m_TapStartPositionInput.EnableDirectActionIfModeUsed();
            m_DeleteButton.onClick.AddListener(DeleteFocusedObject);
        }

        void OnDisable()
        {
            m_TapStartPositionInput.DisableDirectActionIfModeUsed();
            m_DeleteButton.onClick.RemoveListener(DeleteFocusedObject);
        }

        void Update()
        {
            if (m_InteractionGroup != null)
            {
                var currentFocusedObject = m_InteractionGroup.focusInteractable;
                if (currentFocusedObject != null && (!m_DeleteButton.isActiveAndEnabled))
                {
                    m_DeleteButton.gameObject.SetActive(true);
                }
                else if (currentFocusedObject == null && m_DeleteButton.isActiveAndEnabled)
                {
                    m_DeleteButton.gameObject.SetActive(false);
                }
            }
        }

        public void ToggleDebugPlane()
        {
            _showDebugPlane = !_showDebugPlane;
            foreach (var trackable in _planeManager.trackables)
            {
                var meshRenderer = trackable.GetComponent<MeshRenderer>();
                if (meshRenderer.materials.Length > 0) 
                {
                    _planeMaterials = meshRenderer.materials;
                }
                meshRenderer.materials = _showDebugPlane ? _planeMaterials : Array.Empty<Material>();
            }
        }

        public void SetObjectToSpawn(int objectIndex)
        {
            if (m_ObjectSpawner == null)
            {
                Debug.LogWarning("Menu Manager not configured correctly: no Object Spawner set.", this);
            }
            else
            {
                if (objectIndex < m_ObjectSpawner.objectPrefabs.Count)
                {
                    m_ObjectSpawner.spawnOptionIndex = objectIndex;
                }
                else
                {
                    Debug.LogWarning("Object Spawner not configured correctly: object index larger than number of Object Prefabs.", this);
                }
            }
        }
        
        void DeleteFocusedObject()
        {
            if (m_InteractionGroup == null)
                return;

            var currentFocusedObject = m_InteractionGroup.focusInteractable;
            if (currentFocusedObject != null)
            {
                Destroy(currentFocusedObject.transform.gameObject);
            }
        }
    }
}
