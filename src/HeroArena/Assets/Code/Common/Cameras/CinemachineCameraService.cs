using Unity.Cinemachine;
using UnityEngine;

namespace Code.Common.Cameras
{
    public class CinemachineCameraService : MonoBehaviour, ICameraService
    {
        [SerializeField] private CinemachineCamera _cinemachineCamera;

        public void SetTarget(Transform target) => _cinemachineCamera.Follow = target;

        public bool HasTarget() => _cinemachineCamera.Follow != null;
    }
}