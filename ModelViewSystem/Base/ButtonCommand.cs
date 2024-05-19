using System;
using System.Windows.Input;

namespace ModelViewSystem
{
	/// <summary>
	/// Команда для выполнения действия при нажатии на кнопку.
	/// </summary>
	public class ButtonCommand : ICommand
	{
		private readonly Action<object> _executeAction;

		// Конструктор класса, принимающий делегат Action<object> для выполнения действия
		public ButtonCommand(Action<object> executeAction)
		{
			_executeAction = executeAction;
		}

		// Событие CanExecuteChanged, необходимое для определения возможности выполнения команды
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
		/// <summary>
		/// Метод CanExecute определяет, может ли команда быть выполнена в текущем состоянии
		/// </summary>
		public bool CanExecute(object parameter) => _executeAction != null;
		/// <summary>
		/// Метод Execute выполняет заданное действие при вызове команды
		/// </summary>
		public void Execute(object parameter) => _executeAction(parameter);
	}
}
