using UnityEngine;

namespace Code.Common.Cameras
{
    public interface ICameraService
    {
        Camera GetCamera();

        void SetTarget(Transform target);

        bool HasTarget();
    }
}