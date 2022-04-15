using System;
using System.Threading;
using System.Threading.Tasks;
using Bookinist.Infrastructure.Commands.Base;

namespace Bookinist.Infrastructure.Commands;

internal class LambdaCommandAsync : Command
{
    private readonly Func<object, Task> _executeAsync;
    private readonly Func<object, bool> _canExecuteAsync;

    private volatile Task _executingTask;

    public bool Background { get; set; }

    public LambdaCommandAsync(Func<Task> executeAsync, Func<bool> canExecute)
        : this(
            executeAsync is null
                ? throw new ArgumentNullException(nameof(executeAsync))
                : new Func<object, Task>(_ => executeAsync()),
            canExecute is null ? null : _ => canExecute())
    {
    }

    public LambdaCommandAsync(Func<object, Task> executeAsync, Func<bool> canExecute = null)
        : this(executeAsync, canExecute is null ? null : _ => canExecute())
    {
    }

    public LambdaCommandAsync(Func<object, Task> executeAsync, Func<object, bool> canExecuteAsync = null)
    {
        _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        _canExecuteAsync = canExecuteAsync;
    }

    protected override bool CanExecute(object parameter) =>
        (_executingTask is null || _executingTask.IsCompleted)
        && (_canExecuteAsync?.Invoke(parameter) ?? true);

    protected override async void Execute(object parameter)
    {
        var background = Background;

        var canExecute = background
            ? await Task.Run(() => CanExecute(parameter))
            : CanExecute(parameter);

        if (!canExecute) return;

        Task executeAsync = background ? Task.Run(() => _executeAsync(parameter)) : _executeAsync(parameter);
        _ = Interlocked.Exchange(ref _executingTask, executeAsync);
        _executingTask = executeAsync;
        OnCanExecuteChanged();

        try
        {
            await executeAsync.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
        }

        OnCanExecuteChanged();
    }
}