using System;
using System.Windows.Input;

namespace Bookinist.Infrastructure.Commands.Base;

internal abstract class Command : ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add
        {
            CommandManager.RequerySuggested += value;
            CanExecuteChangedHandlers += value;
        }
        remove
        {
            CommandManager.RequerySuggested -= value;
            CanExecuteChangedHandlers -= value;
        }
    }

    private bool _executable = true;
    private event EventHandler CanExecuteChangedHandlers;

    protected virtual void OnCanExecuteChanged(EventArgs e = null) =>
        CanExecuteChangedHandlers?.Invoke(this, e ?? EventArgs.Empty);

    public bool Executable
    {
        get => _executable;
        set
        {
            if (_executable == value) return;
            _executable = value;
            CommandManager.InvalidateRequerySuggested();
        }
    }

    bool ICommand.CanExecute(object parameter) => _executable && CanExecute(parameter);

    void ICommand.Execute(object parameter)
    {
        if (CanExecute(parameter))
            Execute(parameter);
    }

    protected virtual bool CanExecute(object parameter) => true;

    protected abstract void Execute(object parameter);
}