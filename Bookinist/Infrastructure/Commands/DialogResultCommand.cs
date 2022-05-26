using System;
using System.Windows;
using System.Windows.Input;

namespace Bookinist.Infrastructure.Commands;

internal class DialogResultCommand : ICommand
{
    #region Properties

    public bool? DialogResult { get; set; }

    #endregion

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter) => App.CurrentWindow != null;

    public void Execute(object parameter)
    {
        if (!CanExecute(parameter)) return;

        Window window = App.CurrentWindow;
        var dialogResult = DialogResult;

        if (parameter != null)
            dialogResult = Convert.ToBoolean(parameter);

        window.DialogResult = dialogResult;
        window.Close();
    }
}