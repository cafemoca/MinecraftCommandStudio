using System.Windows;
using System.Windows.Input;

namespace Cafemoca.MinecraftCommandStudio.Views.Behaviors
{
    public static class DropFileBehavior
    {
        private static readonly DependencyProperty DropFileProperty =
            DependencyProperty.RegisterAttached("DropFile", typeof(ICommand), typeof(DropFileBehavior),
            new PropertyMetadata(null, OnDropFileChanged));

        public static void SetDropFile(DependencyObject source, ICommand value)
        {
            source.SetValue(DropFileProperty, value);
        }

        public static ICommand GetDropFile(DependencyObject source)
        {
            return (ICommand)source.GetValue(DropFileProperty);
        }

        private static void OnDropFileChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var element = obj as UIElement;
            element.Drop -= UIElement_Drop;

            var command = e.NewValue as ICommand;
            if (command != null)
            {
                element.Drop += UIElement_Drop;
            }
        }

        private static void UIElement_Drop(object sender, DragEventArgs e)
        {
            var element = sender as UIElement;
            if (element == null)
            {
                return;
            }

            var dropCommand = GetDropFile(element);
            if (dropCommand == null)
            {
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var paths = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (var path in paths)
                {
                    if (dropCommand is RoutedCommand)
                    {
                        (dropCommand as RoutedCommand).Execute(path, element);
                    }
                    else
                    {
                        dropCommand.Execute(path);
                    }
                }
            }
        }
    }
}
