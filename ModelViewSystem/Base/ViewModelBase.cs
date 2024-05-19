using DatabaseManagers;
using System.ComponentModel;
using System.Windows;

namespace ModelViewSystem
{
	/// <summary>
	/// Базовый класс для ViewModel, реализующий интерфейс INotifyPropertyChanged для уведомления об изменениях свойств.
	/// </summary>
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public ViewModelBase()
		{

		}
	}
}
