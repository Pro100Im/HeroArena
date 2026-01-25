using System.Collections.Generic;
using UnityEngine;

namespace Code.Common.Windows.Configs
{
    [CreateAssetMenu(fileName = "WindowConfig", menuName = "Configs/Window Configs")]
    public class WindowsConfig : ScriptableObject
    {
        public List<WindowConfig> WindowConfigs;
    }
}