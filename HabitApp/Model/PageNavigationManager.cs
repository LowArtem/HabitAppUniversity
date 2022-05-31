using System;
using System.Windows.Controls;

namespace HabitApp.Model
{
    public class PageNavigationManager
    {
        public event Action<UserControl> OnPageChanged;
        public void ChangePage(UserControl page) => OnPageChanged?.Invoke(page);

        internal void ChangePage(object getRequiredServices)
        {
            throw new NotImplementedException();
        }
    }
}
