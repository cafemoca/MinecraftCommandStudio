using System;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Cafemoca.MinecraftCommandStudio.Services
{
    public static class DialogService
    {
        private static MetroWindow Window
        {
            get { return (App.Current.MainWindow as MetroWindow); }
        }

        public static async Task<MessageDialogResult> ShowMessageAsync
            (string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            return await Window.ShowMessageAsync(
                title, message, style, Window.MetroDialogOptions);
        }

        public static async Task<string> ShowInputAsync
            (string title, string message)
        {
            Window.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;
            return await Window.ShowInputAsync(
                title, message, Window.MetroDialogOptions);
        }

        public static async Task<ProgressDialogController> ShowProgressAsync
            (string title, string message, bool isCancelable)
        {
            Window.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;
            return await Window.ShowProgressAsync(
                title, message, isCancelable, Window.MetroDialogOptions);
        }

        public static async Task ShowMetroDialogAsync(string key)
        {
            Window.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;
            var dialog = Window.Resources[key] as BaseMetroDialog;
            if (dialog == null)
            {
                throw new ArgumentNullException();
            }
            await Window.ShowMetroDialogAsync(dialog);
        }

        public static async Task HideMetroDialogAsync(string key)
        {
            Window.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;
            var dialog = Window.Resources[key] as BaseMetroDialog;
            if (dialog == null)
            {
                throw new ArgumentNullException();
            }
            await Window.HideMetroDialogAsync(dialog);
        }
    }
}
