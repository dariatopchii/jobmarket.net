using JobMarket.Files.Interfaces;
using JobMarket.Files.ReaderWriters;
using JobMarket.Files.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace JobMarket.Files
{
    public static class DependencyRegistrar
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericStorageWorker<>), typeof(GenericJsonWorker<>));
            services.AddScoped<IReaderWriter, JsonReaderWriter>();
        }
    }
}

