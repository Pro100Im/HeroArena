using UnityEngine;

namespace Code.Common.Windows
{
    public interface IWindowFactory
    {
        public void SetUIRoot(RectTransform uiRoot);
        public BaseWindow CreateWindow(WindowId windowId);
    }
}