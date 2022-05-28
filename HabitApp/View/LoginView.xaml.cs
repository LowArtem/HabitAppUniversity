using HabitApp.CustomControls;
using System.Windows.Controls;

namespace HabitApp.View
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordTextBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            // Динамическое изменения поля Password в VM
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).Password = ((HintTextBox)sender).password.Password;
            }
        }
    }
}
