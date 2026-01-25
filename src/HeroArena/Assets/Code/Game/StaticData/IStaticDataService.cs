using Code.Common.Windows;
using UnityEngine;

namespace Code.Common.StaticData
{
    public interface IStaticDataService
    {
        public void LoadAll();

        public GameObject GetWindowPrefab(WindowId id);
    }
}