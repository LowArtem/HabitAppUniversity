using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitApp.Model
{
    public interface ICommand
    {
        bool CanExecute(object parameter);

        void Execute(object parameter);
    }
}
