using HabitApp.Model;
using MaterialDesignThemes.Wpf;
using System.Runtime.CompilerServices;

namespace HabitApp.VM
{
    public class CompletionRatingDialogVM : ViewModel
    {
        public CompletionRatingDialogVM()
        {
            Rating = 3;
        }

        #region Rating : int - Оценка за выполнение привычки

        /// <summary>Оценка за выполнение привычки</summary>
        private int _Rating;

        /// <summary>Оценка за выполнение привычки</summary>
        public int Rating
        {
            get => _Rating;
            set => Set(ref _Rating, value);
        }

        #endregion

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Rating))
            {
                DialogHost.CloseDialogCommand.Execute(Rating.ToString(), null);
            }
        }
    }
}
