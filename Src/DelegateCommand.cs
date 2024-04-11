using System;
using System.Windows.Input;

namespace ColorShadesGenerator;

public class DelegateCommand : ICommand
{
    #region Constants and Fields

    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    #endregion

    #region Constructors and Destructors

    public DelegateCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    #endregion

    #region ICommand Implementation

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke() ?? true;
    }

    public void Execute(object? parameter)
    {
        _execute.Invoke();
    }

    #endregion

    #region Public Methods

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion
}