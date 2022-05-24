using HabitApp.Model;
using HabitApp.Services;
using HabitApp.View;
using System.Windows.Input;

namespace HabitApp.VM
{
    public class MainWindowVM : ViewModel
    {
        private readonly LoginService _loginService;
        public MainWindowVM(LoginService loginService)
        {
            _loginService = loginService;

            OpenSomenthingCommand = new BaseCommand(OnOpenSomenthingCommandExecuted, CanOpenSomenthingCommandExecute);
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


        #region CurrentView : object - Выбранное окно на экране

        /// <summary>Выбранное окно на экране</summary>
        private object _CurrentView = new HomeView();

        /// <summary>Выбранное окно на экране</summary>
        public object CurrentView
        {
            get => _CurrentView;
            set => Set(ref _CurrentView, value);
        }

        #endregion


        #region OpenSomenthingCommand

        public ICommand OpenSomenthingCommand { get; }
        private bool CanOpenSomenthingCommandExecute(object p) => true;

        private void OnOpenSomenthingCommandExecuted(object p)
        {
            CurrentView = new LoginView();
        }

        #endregion

    }
}
