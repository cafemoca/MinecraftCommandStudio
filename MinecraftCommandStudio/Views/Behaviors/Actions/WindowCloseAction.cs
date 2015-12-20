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
            get { return (bool)this.GetValue(WindowCloseProperty); }
            set { this.SetValue(WindowCloseProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            if (this.WindowClose)
            {
                var window = Window.GetWindow(this.AssociatedObject);
                window.Close();
            }
        }
    }
}
