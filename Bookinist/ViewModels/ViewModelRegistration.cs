using Bookinist.ViewModels.MainWindowVm;
using Microsoft.Extensions.DependencyInjection;

namespace Bookinist.ViewModels
{
    public static class ViewModelRegistration
    {
        public static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();
            return services;
        }
    }
}