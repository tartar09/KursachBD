using ModelViewSystem;
using System.Windows.Controls;
using System.Windows.Data;

namespace PasswordChangeMenu
{
	/// <summary>
	/// Логика взаимодействия для PasswordChangePage.xaml
	/// </summary>
	public partial class PasswordChangePage : Page
	{
		public PasswordChangePage()
		{
			InitializeComponent();
			DataContext = new PasswordPageModelView();
			Binding();
		}

		private void Binding()
		{
			LoginBox.SetBinding(TextBox.TextProperty, new Binding("Login") { Mode = BindingMode.TwoWay, Source = DataContext } );
			PasswordBox.SetBinding(TextBox.TextProperty, new Binding("Password") { Mode = BindingMode.TwoWay, Source= DataContext });
			ChangeButton.SetBinding(Button.CommandProperty, new Binding("ButtonCommand") { Source = DataContext });
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e) => WindowEvents.UserLogOut();
	}
}
