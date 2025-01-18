using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Utilities;

namespace SnakeWorks
{
    public class ARObjectManager : MonoBehaviour
    {
        public static ARObjectManager Instance { get; private set; }

        [SerializeField] private XRRayInteractor _arInteractor;
        [SerializeField] private float _viewportPeriphery = 0.15f;

        public XRRayInteractor Interactor => _arInteractor;

        private Camera _camera;

        void Awake()
        {
            Instance = this;
            _camera = Camera.main;
        }

        public GameObject TrySpawnObjectRaycast(GameObject prefab)
        {
            var isPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1);
            if (!isPointerOverUI && _arInteractor.TryGetCurrentARRaycastHit(out var arRaycastHit))
            {
                if (arRaycastHit.trackable is not ARPlane arPlane)
                {
                    return null;
                }
                return TrySpawnObject(prefab, arRaycastHit.pose.position, arPlane.normal);
            }
            return null;
        }

        public GameObject TrySpawnObject(GameObject prefab, Vector3 spawnPoint, Vector3 spawnNormal)
        {
            var inViewMin = _viewportPeriphery;
            var inViewMax = 1f - _viewportPeriphery;
            var pointInViewportSpace = _camera.WorldToViewportPoint(spawnPoint);
            if (pointInViewportSpace.z < 0f || pointInViewportSpace.x > inViewMax || pointInViewportSpace.x < inViewMin ||
                pointInViewportSpace.y > inViewMax || pointInViewportSpace.y < inViewMin)
            {
                return null;
            }

            var newObject = Instantiate(prefab);
            newObject.transform.position = spawnPoint;

            var facePosition = _camera.transform.position;
            var forward = facePosition - spawnPoint;
            BurstMathUtility.ProjectOnPlane(forward, spawnNormal, out var projectedForward);
            newObject.transform.rotation = Quaternion.LookRotation(projectedForward, spawnNormal);

            return newObject;
        }
    }
}
