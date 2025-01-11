using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ITHS_DB_Labb03.Core;

internal class RelayCommand : ICommand
{
    private readonly Action<object> execute;
    private readonly Func<object?, bool> canExecute;

    public event EventHandler? CanExecuteChanged;


    public RelayCommand(Action<object> execute, Func<object?, bool> canExecute = null)
    {
        ArgumentNullException.ThrowIfNull(execute);

        this.execute = execute;
        this.canExecute = canExecute;
    }

    public void RaisedCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    public bool CanExecute(object? parameter)
    {
        return canExecute is null ? true : canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        execute(parameter);
    }


}

