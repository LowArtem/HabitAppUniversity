using HabitApp.Model;
using HabitApp.View;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HabitApp.VM
{
    public class DashboardVM : ViewModel
    {
        private readonly PageNavigationManager _pageNavigationManager;

        public DashboardVM(PageNavigationManager pageNavigationManager = null)
        {
            _pageNavigationManager = pageNavigationManager;

            ExitFromApplicationCommand = new BaseCommand(OnExitFromApplicationCommandExecuted, CanExitFromApplicationCommandExecute);
            LogoutCommand = new BaseCommand(OnLogoutCommandExecuted, CanLogoutCommandExecute);
            NavigateToHomeViewCommand = new BaseCommand(OnNavigateToHomeViewCommandExecuted, CanNavigateToHomeViewCommandExecute);
        }

        #region Username : string - Имя текущего пользователя

        /// <summary>Имя текущего пользователя</summary>
        private string _Username = (Application.Current as App).CurrentUser.Username;

        /// <summary>Имя текущего пользователя</summary>
        public string Username
        {
            get => _Username;
        }

        #endregion




        #region ExitFromApplicationCommand

        public ICommand ExitFromApplicationCommand { get; }
        private bool CanExitFromApplicationCommandExecute(object p) => true;

        private void OnExitFromApplicationCommandExecuted(object p)
        {
            App.Current.Shutdown();
        }

        #endregion

        #region LogoutCommand

        public ICommand LogoutCommand { get; }
        private bool CanLogoutCommandExecute(object p) => true;

        private void OnLogoutCommandExecuted(object p)
        {
            Properties.Settings.Default.userId = -1;
            Properties.Settings.Default.Save();

            _pageNavigationManager.ChangePage(App.Host.Services.GetRequiredService<LoginView>());
        }

        #endregion

        #region NavigateToHomeViewCommand

        public ICommand NavigateToHomeViewCommand { get; }
        private bool CanNavigateToHomeViewCommandExecute(object p) => true;

        private void OnNavigateToHomeViewCommandExecuted(object p)
        {
            _pageNavigationManager.ChangePage(App.Host.Services.GetRequiredService<HomeView>());
        }

        #endregion

    }
}
