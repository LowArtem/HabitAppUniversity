using HabitApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitApp.VM
{
    public class MainVM : ViewModel
    {
        public MainVM()
        {

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

    }
}
