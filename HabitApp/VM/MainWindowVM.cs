using HabitApp.Model;
using HabitApp.Services;
using HabitApp.View;
using System.Windows.Controls;
using System.Windows.Input;

namespace HabitApp.VM
{
    public class MainWindowVM : ViewModel
    {
        private readonly LoginService _loginService;
        private readonly PageNavigationManager _pageNavigationManager;

        public MainWindowVM(LoginService loginService, PageNavigationManager pageNavigationManager)
        {
            _loginService = loginService;
            _pageNavigationManager = pageNavigationManager;

            _pageNavigationManager.OnPageChanged += (page) => CurrentView = page;
            _pageNavigationManager.ChangePage(new HomeView());
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


        #region CurrentView : UserControl - Выбранное окно на экране

        /// <summary>Выбранное окно на экране</summary>
        private UserControl _CurrentView;

        /// <summary>Выбранное окно на экране</summary>
        public UserControl CurrentView
        {
            get => _CurrentView;
            set => Set(ref _CurrentView, value);
        }

        #endregion
    }
}
