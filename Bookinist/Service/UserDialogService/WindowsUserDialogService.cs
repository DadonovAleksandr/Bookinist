using Bookinist.DAL.Entityes;
using Bookinist.ViewModels;
using Bookinist.Views;
using Bookinist.Views.Windows;
using System;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Bookinist.Service.UserDialogService
{
    internal class WindowsUserDialogService : IUserDialogService
    {
        private static Window ActiveWindow => Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.IsActive);

        private static Window FocusedWindow => Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w.IsFocused);

        public static Window CurrentWindow => FocusedWindow ?? ActiveWindow;

        public bool Edit (Book book)
        {
            var bookEditorModel = new BookEditorWindowViewModel(book);
            var bookEditorWindow = new BookEditorWindow
            {
                DataContext = bookEditorModel
            };
            if(bookEditorWindow.ShowDialog() != true )
                return false;

            book.Name = bookEditorModel.Name;
            return true;
        }

        public void ShowInformation(string message) => MessageBox
            .Show(ActiveWindow, message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        public void ShowWarning(string message) => ShowErrorWindow(message, ErrorWindowType.Warning);
        public void ShowError(string message) => ShowErrorWindow(message, ErrorWindowType.Error);

        private void ShowErrorWindow(string msg, ErrorWindowType type)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var dlg = new ErrorWindow(msg, type)
                {
                    Owner = CurrentWindow
                };
                dlg.ShowDialog();
            });
        }

        public bool Confirm(string message, bool exclamation = false) => MessageBox
            .Show(message, "Запрос пользователю", MessageBoxButton.YesNo,
                exclamation ? MessageBoxImage.Exclamation : MessageBoxImage.Question) == MessageBoxResult.Yes;

        public (IProgress<double> Progress, IProgress<string> Status, CancellationToken Cancel, Action Close) ShowProgress(string title)
        {
            var progressWindow = new ProgressWindow { Title = title, Owner = CurrentWindow, WindowStartupLocation = WindowStartupLocation.CenterOwner };
            progressWindow.Show();
            return (progressWindow.ProgressInformer, progressWindow.StatusInformer, progressWindow.Cancellation, progressWindow.Close);
        }
    }
}