using HabitApp.Data;
using HabitApp.Model;
using HabitApp.View;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace HabitApp.VM
{
    public class MainWindowVM : ViewModel
    {
        private readonly PageNavigationManager _pageNavigationManager;

        public MainWindowVM(PageNavigationManager pageNavigationManager)
        {
            _pageNavigationManager = pageNavigationManager;

            _pageNavigationManager.OnPageChanged += (page) => CurrentView = page;

            OpenFirstPage();
        }

        private void OpenFirstPage()
        {
            if (Properties.Settings.Default.userId == -1) // нет данных или данные были очищены
            {
                _pageNavigationManager.ChangePage(App.Host.Services.GetRequiredService<LoginView>());
                return;
            }

            int? groupId = null;
            if (Properties.Settings.Default.userGroupId != -1) groupId = Properties.Settings.Default.userGroupId;

            User user = new User(
                    id: Properties.Settings.Default.userId,
                    username: Properties.Settings.Default.userUsername,
                    password: Properties.Settings.Default.userPassword,
                    experience: Properties.Settings.Default.userExperience,
                    money: Properties.Settings.Default.userMoney,
                    groupId: groupId
                );

            (Application.Current as App).CurrentUser = user;

            _pageNavigationManager.ChangePage(App.Host.Services.GetRequiredService<HomeView>());
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
