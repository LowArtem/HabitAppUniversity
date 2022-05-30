using HabitApp.VM;
using System.Windows.Controls;
using System.Windows.Markup;

namespace HabitApp.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();

            TaskDeadlineDatePicker.Language = XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag);


            if (this.DataContext != null)
            {
                ((HomeVM)this.DataContext).PropertyChanged += HomeVM_PropertyChanged;
            }
        }
        private void HomeVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HomeVM.Habits))
            {
                this.HabitListBox.Items.Refresh();
            }
            else if (e.PropertyName == nameof(HomeVM.DailyHabits))
            {
                this.DailyHabitListBox.Items.Refresh();
            }
            else if (e.PropertyName == nameof(HomeVM.Tasks))
            {
                this.TaskListBox.Items.Refresh();
            }
        }
    }
}
