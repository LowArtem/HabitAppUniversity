using HabitApp.Model;
using HabitApp.Services;
using HabitApp.View;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace HabitApp.VM
{
    public class LoginVM : ViewModel
    {
        private readonly LoginService _loginService;
        private readonly PageNavigationManager _pageNavigationManager;

        public LoginVM(LoginService loginService, PageNavigationManager pageNavigationManager)
        {
            LoginCommand = new BaseCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
            RegisterCommand = new BaseCommand(OnRegisterCommandExecuted, CanRegisterCommandExecute);

            _loginService = loginService;
            _pageNavigationManager = pageNavigationManager;
        }

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
            //return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
            return true;
        }

        private void OnLoginCommandExecuted(object p)
        {
            // TODO: подумать, как это можно улучшить, видимо, в этом и проблема активности кнопок

            Password = ((CustomControls.HintTextBox)p).password.Password;

            bool result = _loginService.Login(Username, Password);

            if (result)
            {
                _pageNavigationManager.ChangePage(App.Host.Services.GetRequiredService<HomeView>());
            }
            else
            {
                MessageBox.Show("Операция входа не удалась. Возможно вы ввели неверные данные");
            }
        }

        #endregion

        #region RegisterCommand

        public ICommand RegisterCommand { get; }
        private bool CanRegisterCommandExecute(object p)
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        private void OnRegisterCommandExecuted(object p)
        {
            bool result = _loginService.Register(Username, Password);

            if (result)
            {
                _pageNavigationManager.ChangePage(App.Host.Services.GetRequiredService<HomeView>());
            }
            else
            {
                MessageBox.Show("Операция входа не удалась. Возможно вы ввели неверные данные");
            }
        }

        #endregion            

    }
}
