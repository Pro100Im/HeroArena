using Unity.Cinemachine;
using UnityEngine;

namespace Code.Common.Cameras
{
    public class CinemachineCameraService : MonoBehaviour, ICameraService
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CinemachineCamera _cinemachineCamera;

        public Camera GetCamera() => _camera;

        public void SetTarget(Transform target) => _cinemachineCamera.Follow = target;

        public bool HasTarget() => _cinemachineCamera.Follow != null;
    }
}