using Code.Common.Windows;
using Code.Common.Windows.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Common.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<WindowId, GameObject> _windowPrefabsById;

        public StaticDataService()
        {
            LoadAll();
        }

        public void LoadAll()
        {
            LoadWindows();
        }

        public GameObject GetWindowPrefab(WindowId id) => _windowPrefabsById.TryGetValue(id, out GameObject prefab)
            ? prefab
            : throw new Exception($"Prefab config for window {id} was not found");

        private void LoadWindows()
        {
            _windowPrefabsById = Resources
              .Load<WindowsConfig>("Configs/WindowConfigs/WindowConfig")
              .WindowConfigs
              .ToDictionary(x => x.Id, x => x.Prefab);
        }
    }
}