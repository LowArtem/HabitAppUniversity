using Microsoft.Extensions.DependencyInjection;

namespace HabitApp.VM
{
    public class VMLocator
    {
        public MainWindowVM MainWindowVM => App.Host.Services.GetRequiredService<MainWindowVM>();
        public HomeVM HomeVM => App.Host.Services.GetRequiredService<HomeVM>();
        public LoginVM LoginVM => App.Host.Services.GetRequiredService<LoginVM>();
        public CompletionRatingDialogVM CompletionRatingDialogVM => App.Host.Services.GetRequiredService<CompletionRatingDialogVM>();
    }
}
