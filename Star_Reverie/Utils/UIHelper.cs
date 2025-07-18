using Stride.Engine;
using Stride.UI;

namespace Star_Reverie.Utils
{
    public static class UIHelper
    {
        public static bool IsPointerOverUI(UIComponent uiComponent)
        {
            if (uiComponent == null || !uiComponent.Enabled || uiComponent.Page == null)
                return false;

            UIElement rootElement = uiComponent.Page.RootElement;
            if (rootElement == null || !rootElement.IsVisible)
                return false;

            return rootElement.MouseOverState != MouseOverState.MouseOverNone;
        }
    }
}
