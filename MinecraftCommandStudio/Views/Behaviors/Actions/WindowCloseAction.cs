using System.Windows;
using System.Windows.Interactivity;

namespace Cafemoca.MinecraftCommandStudio.Views.Behaviors.Actions
{
    /// <summary>
    /// Original: http://www.makcraft.com/blog/meditation/2013/09/01/window-close-in-the-mvvm/
    /// </summary>
    class WindowCloseAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty WindowCloseProperty = DependencyProperty.Register(
            "WindowClose", typeof(bool), typeof(WindowCloseAction), new UIPropertyMetadata(false));

        public bool WindowClose
        {
            get { return (bool)GetValue(WindowCloseProperty); }
            set { SetValue(WindowCloseProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            if (WindowClose)
            {
                var window = Window.GetWindow(AssociatedObject);
                window.Close();
            }
        }
    }
}
