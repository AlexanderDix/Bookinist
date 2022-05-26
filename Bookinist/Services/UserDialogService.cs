using System.Windows;
using Bookinist.DAL.Entities;
using Bookinist.Services.Interfaces;
using Bookinist.ViewModels.WindowViewModels;
using Bookinist.Views.Windows;

namespace Bookinist.Services;

internal class UserDialogService : IUserDialog
{
    public bool Edit(Book book)
    {
        var bookEditorModel = new BookEditorViewModel(book);
        var bookEditorWindow = new BookEditorWindow
        {
            DataContext = bookEditorModel
        };

        if (bookEditorWindow.ShowDialog() != true) return false;

        book.Name = bookEditorModel.Name;

        return true;
    }

    public bool ConfirmInformation(string information, string caption) =>
        MessageBox.Show(information, caption, MessageBoxButton.YesNo, MessageBoxImage.Information) ==
        MessageBoxResult.Yes;

    public bool ConfirmWarning(string warning, string caption) =>
        MessageBox.Show(warning, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
        MessageBoxResult.Yes;

    public bool ConfirmError(string error, string caption) =>
        MessageBox.Show(error, caption, MessageBoxButton.YesNo, MessageBoxImage.Error) ==
        MessageBoxResult.Yes;
}