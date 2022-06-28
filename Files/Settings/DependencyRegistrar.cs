
using JobMarket.Files.GenericCollection;
using JobMarket.Files.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace JobMarket.Files
{
    public static class DependencyRegistrar
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericCollection<>), typeof(GenericCollection<>));
        }
    }
}

