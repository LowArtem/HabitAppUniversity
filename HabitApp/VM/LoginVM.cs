using HabitApp.Data;
using HabitApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitApp.VM
{
    public class LoginVM : ViewModel
    {
        private User currentUser = null;

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand Login { get; }
        private void OnLoginExecuted(object p)
        {
            // тут ещё подумать
        }
    }
}
