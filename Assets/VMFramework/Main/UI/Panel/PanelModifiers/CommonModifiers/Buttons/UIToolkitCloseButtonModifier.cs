namespace VMFramework.UI
{
    public sealed class UIToolkitCloseButtonModifier : UIToolkitButtonModifier
    {
        protected override void OnClicked()
        {
            Panel.Close();
        }
    }
}