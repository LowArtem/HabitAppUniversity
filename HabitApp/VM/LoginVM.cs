using HabitApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HabitApp.VM
{
    public class LoginVM : ViewModel
    {

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

        #region CloseApplicationCommand

        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        #endregion

        public LoginVM()
        {
            CloseApplicationCommand = new BaseCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
        }
    }
}
