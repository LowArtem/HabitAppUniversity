using HabitApp.Model;

namespace HabitApp.VM
{
    public class HomeVM : ViewModel
    {
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

    }
}
