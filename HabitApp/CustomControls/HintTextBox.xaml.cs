using System.Windows;
using System.Windows.Controls;

namespace HabitApp.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для HintTextBox.xaml
    /// </summary>
    public partial class HintTextBox : UserControl
    {
        public HintTextBox()
        {
            InitializeComponent();
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(HintTextBox));

        public string Text
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(HintTextBox));

        public bool IsPassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty IsPasswordProperty =
            DependencyProperty.Register("IsPassword", typeof(bool), typeof(HintTextBox));

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            username.Text = password.Password;
        }
    }
}
