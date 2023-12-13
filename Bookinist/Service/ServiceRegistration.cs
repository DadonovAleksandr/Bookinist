using Bookinist.Service.UserDialogService;
using Microsoft.Extensions.DependencyInjection;

namespace Bookinist.Service;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterServices(this IServiceCollection services) => services
        .AddTransient<IUserDialogService, WindowsUserDialogService>()
        ;
        
}