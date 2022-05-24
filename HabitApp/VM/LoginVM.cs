using HabitApp.Model;
using HabitApp.Services;
using HabitApp.View;
using System.Windows.Input;

namespace HabitApp.VM
{
    public class LoginVM : ViewModel
    {
        private readonly LoginService _loginService;
        public LoginVM(LoginService loginService)
        {
            LoginCommand = new BaseCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
            RegisterCommand = new BaseCommand(OnRegisterCommandExecuted, CanRegisterCommandExecute);

            _loginService = loginService;
        }

        #region Title : string - Заголовок окна

        /// <summary>Заголовок окна</summary>
        private string _Title = "Habit App";

        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        #region Username : string - Имя пользователя

        /// <summary>Имя пользователя</summary>
        private string _Username;

        /// <summary>Имя пользователя</summary>
        public string Username
        {
            get => _Username;
            set => Set(ref _Username, value);
        }

        #endregion

        #region Password : string - Пароль

        /// <summary>Пароль</summary>
        private string _Password;

        /// <summary>Пароль</summary>
        public string Password
        {
            get => _Password;
            set => Set(ref _Password, value);
        }

        #endregion

        #region LoginCommand

        public ICommand LoginCommand { get; }
        private bool CanLoginCommandExecute(object p)
        {
            return !string.IsNullOrEmpty(_Username) && !string.IsNullOrEmpty(_Password);
        }

        private void OnLoginCommandExecuted(object p)
        {
            bool result = _loginService.Login(_Username, _Password);

            if (result)
            {
                MainWindow mainWindow = new MainWindow();
                // разобраться с навигацией
                mainWindow.Show();
            }
            else
            {
                // Вывести сообщение об ошибке
            }
        }

        #endregion

        #region RegisterCommand

        public ICommand RegisterCommand { get; }
        private bool CanRegisterCommandExecute(object p)
        {
            return !string.IsNullOrEmpty(_Username) && !string.IsNullOrEmpty(_Password);
        }

        private void OnRegisterCommandExecuted(object p)
        {
            bool result = _loginService.Register(_Username, _Password);

            if (result)
            {
                MainWindow mainWindow = new MainWindow();
                // разобраться с навигацией
                mainWindow.Show();
            }
            else
            {
                // Вывести сообщение об ошибке
            }
        }

        #endregion
    }
}
