using HabitApp.Model;
using HabitApp.Services;
using HabitApp.View;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System;
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

            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
        }

        private void SaveCredentials(int id, string username, string password, long money, long experience, int groupid = -1)
        {
            Properties.Settings.Default.userId = id;
            Properties.Settings.Default.userUsername = username;
            Properties.Settings.Default.userPassword = password;
            Properties.Settings.Default.userMoney = money;
            Properties.Settings.Default.userExperience = experience;
            Properties.Settings.Default.userGroupId = groupid;

            Properties.Settings.Default.Save();
        }

        #region MessageQueue : SnackbarMessageQueue - Сообщение об ошибке

        /// <summary>Сообщение об ошибке</summary>
        private SnackbarMessageQueue _MessageQueue;

        /// <summary>Сообщение об ошибке</summary>
        public SnackbarMessageQueue MessageQueue
        {
            get => _MessageQueue;
            set => Set(ref _MessageQueue, value);
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
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        private void OnLoginCommandExecuted(object p)
        {
            bool result = _loginService.Login(Username, Password);

            if (result)
            {
                var user = (Application.Current as App).CurrentUser;
                SaveCredentials(user.Id, user.Username, user.Password, user.Money, user.Experience, user.GroupId ?? -1);

                _pageNavigationManager.ChangePage(App.Host.Services.GetRequiredService<DashboardView>());
            }
            else
            {
                MessageQueue.Enqueue("Login is unsuccessful. Maybe you have entered wrong credentials.");
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
                var user = (Application.Current as App).CurrentUser;
                SaveCredentials(user.Id, user.Username, user.Password, user.Money, user.Experience, user.GroupId ?? -1);

                _pageNavigationManager.ChangePage(App.Host.Services.GetRequiredService<DashboardView>());
            }
            else
            {
                MessageQueue.Enqueue("Register is unsuccessful. Maybe user with these credentials already exists.");
            }
        }

        #endregion            

    }
}
