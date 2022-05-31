using HabitApp.VM;
using System.Windows.Controls;

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

            DailyHabitListBoxSort();
            TaskListBoxSort();

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
                DailyHabitListBoxSort();

                this.DailyHabitListBox.Items.Refresh();
            }
            else if (e.PropertyName == nameof(HomeVM.Tasks))
            {
                TaskListBoxSort();

                this.TaskListBox.Items.Refresh();
            }
        }

        private void TaskListBoxSort()
        {
            this.TaskListBox.Items.SortDescriptions.Clear();
            this.TaskListBox.Items.SortDescriptions.Add(
                new System.ComponentModel.SortDescription("Status", System.ComponentModel.ListSortDirection.Ascending));
        }

        private void DailyHabitListBoxSort()
        {
            this.DailyHabitListBox.Items.SortDescriptions.Clear();
            this.DailyHabitListBox.Items.SortDescriptions.Add(
                new System.ComponentModel.SortDescription("Status", System.ComponentModel.ListSortDirection.Ascending));
        }
    }
}
