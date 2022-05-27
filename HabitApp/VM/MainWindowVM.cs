﻿using HabitApp.Model;
using HabitApp.View;
using Microsoft.Extensions.DependencyInjection;
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
            _pageNavigationManager.ChangePage(App.Host.Services.GetRequiredService<LoginView>());
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
