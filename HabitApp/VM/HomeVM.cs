using HabitApp.Model;
using HabitApp.View;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace HabitApp.VM
{
    public class HomeVM : ViewModel
    {
        private readonly PageNavigationManager _pageNavigationManager;

        public HomeVM(PageNavigationManager pageNavigationManager)
        {
            _pageNavigationManager = pageNavigationManager;

            LoginPageOpenCommand = new BaseCommand(OnLoginPageOpenCommandExecuted, CanLoginPageOpenCommandExecute);
        }

        #region Text : string - Пример текста

        /// <summary>Пример текста</summary>
        private string _Text = "Пример текста";

        /// <summary>Пример текста</summary>
        public string Text
        {
            get => _Text;
            set => Set(ref _Text, value);
        }

        #endregion


        #region LoginPageOpenCommand

        public ICommand LoginPageOpenCommand { get; }
        private bool CanLoginPageOpenCommandExecute(object p) => true;

        private void OnLoginPageOpenCommandExecuted(object p)
        {
            _pageNavigationManager.ChangePage(App.Host.Services.GetRequiredService<LoginView>());
        }

        #endregion

    }
}
