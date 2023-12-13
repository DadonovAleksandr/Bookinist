using Bookinist.ViewModels.MainWindowVm;
using Microsoft.Extensions.DependencyInjection;

namespace Bookinist.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowViewModel => App.Host.Services.GetRequiredService<MainWindowViewModel>();
    }
}