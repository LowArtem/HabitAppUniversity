using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitApp.VM
{
    public class VMLocator
    {
        public LoginVM LoginVM => App.Host.Services.GetRequiredService<LoginVM>();
        public MainVM MainVM => App.Host.Services.GetRequiredService<MainVM>();
    }
}
