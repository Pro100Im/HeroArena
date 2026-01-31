using UnityEngine;

namespace Code.Common.Cameras
{
    public interface ICameraService
    {
        void SetTarget(Transform target);

        bool HasTarget();
    }
}