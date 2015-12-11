using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using TaskDialogInterop;

namespace Cafemoca.MinecraftCommandStudio.Views.Behaviors.Actions
{
    /// <summary>
    /// Original: http://www.makcraft.com/blog/meditation/2013/09/01/window-close-in-the-mvvm/
    /// </summary>
    class WindowClosingAction : TriggerAction<Window>
    {
        protected override void Invoke(object parameter)
        {
            var window = Window.GetWindow(AssociatedObject);

            //if (!Setting.DontAskExitDialog) { ...

            var cancelEventArgs = parameter as CancelEventArgs;
            if (cancelEventArgs == null)
            {
                throw new InvalidOperationException(
                    "WindowClosingAction は EventTrigger の EventName で Closing を指定してください。");
            }

            var inquiryViewModel = window.DataContext as IStateQueryableViewModel;
            if (inquiryViewModel == null)
            {
                throw new InvalidOperationException(
                    "WindowClose の際に ViewModel の状態確認が選択されていますが、ViewModel が IStateQueryableViewModel インターフェイスを実装していません。");
            }

            var cursor = window.Cursor;
            window.Cursor = Cursors.Wait;

            var condition = inquiryViewModel.GetCondition();
            if (condition == CloseCondition.AskCloseTab)
            {
                var dialog = new TaskDialogOptions();
                dialog.Owner = App.MainView;
                dialog.Title = "終了";
                dialog.MainInstruction = "終了しますか？";
                dialog.Content = "";
                dialog.CustomButtons = new[] { "終了 (&E)", "現在のタブのみ閉じる (&A)", "キャンセル (&C)" };
                //dialog.VerificationText = "このメッセージを表示しない";

                var result = TaskDialog.Show(dialog);
                if (result.VerificationChecked.HasValue && result.VerificationChecked.Value)
                {
                    //Setting.DontAskExitDialog = true;
                }
                switch (result.CustomButtonResult)
                {
                    case 0:
                        break;
                    case 1:
                        if (App.MainViewModel.ActiveDocument != null)
                        {
                            App.MainViewModel.ActiveDocument.Value.CloseCommand.Execute();
                        }
                        cancelEventArgs.Cancel = true;
                        window.Cursor = cursor;
                        return;
                    case 2:
                        cancelEventArgs.Cancel = true;
                        window.Cursor = cursor;
                        return;
                }
            }
            else if (condition == CloseCondition.AskSave)
            {
                var dialog = new TaskDialogOptions();
                dialog.Owner = App.MainView;
                dialog.Title = "終了";
                dialog.MainInstruction = "終了しますか？";
                dialog.Content = "現在のドキュメントが未保存です。\n保存しない場合、現在の変更は失われます。";
                dialog.CustomButtons = new[] { "破棄して終了 (&E)", "保存して終了 (&S)", "キャンセル (&C)" };

                var result = TaskDialog.Show(dialog);
                switch (result.CustomButtonResult)
                {
                    case 0:
                        break;
                    case 1:
                        if (App.MainViewModel.ActiveDocument != null)
                        {
                            App.MainViewModel.ActiveDocument.Value.SaveCommand.Execute();
                            if (App.MainViewModel.ActiveDocument.Value.IsModified.Value)
                            {
                                cancelEventArgs.Cancel = true;
                                window.Cursor = cursor;
                                return;
                            }
                        }
                        break;
                    case 2:
                        cancelEventArgs.Cancel = true;
                        window.Cursor = cursor;
                        return;
                }
            }
            else if (condition == CloseCondition.AskExit)
            {
                var count = inquiryViewModel.GetModifiedDocumentCount();
                var dialog = new TaskDialogOptions();
                dialog.Owner = App.MainView;
                dialog.Title = "終了";
                dialog.MainInstruction = "終了しますか？";
                dialog.Content = "未保存のドキュメントが " + count + " 個あります。\n保存しない場合、現在の変更は失われます。";
                dialog.CustomButtons = new[] { "破棄して終了 (&E)", "キャンセル (&C)" };

                var result = TaskDialog.Show(dialog);
                switch (result.CustomButtonResult)
                {
                    case 0:
                        break;
                    case 1:
                        cancelEventArgs.Cancel = true;
                        window.Cursor = cursor;
                        return;
                }
            }

            window.Cursor = cursor;

            var disposableViewModel = window.DataContext as IDisposable;
            if (disposableViewModel != null)
            {
                disposableViewModel.Dispose();
            }
        }
    }
}
