using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyGen.Api.Client;
using Refit;

namespace MyGen.Cmdlets;

internal class Services : IServiceProvider, IDisposable
{
   private static readonly List<ServiceProvider> _serviceProviders = new();
   private readonly ServiceProvider _serviceProvider;

   static Services()
   {
      Instance = new();
   }

   public Services(Action<ServiceCollection, IConfiguration> scBuilder)
   {
      IConfigurationRoot configuration = GetConfiguration();

      var sc = new ServiceCollection();
      scBuilder.Invoke(sc, configuration);
      _serviceProvider = sc.BuildServiceProvider();

      _serviceProviders.Add(_serviceProvider);
   }

   private Services() : this(CreateDefaultServiceCollection)
   {
   }

   public static Services Instance { get; }

   public void Dispose()
   {
      _serviceProviders.Remove(_serviceProvider);
      _serviceProvider.Dispose();
   }

   public object? GetService(Type serviceType)
   {
      for (int i = _serviceProviders.Count - 1; i >= 0; i--)
      {
         if (_serviceProviders[i].GetService(serviceType) is { } s)
         {
            return s;
         }
      }

      return default;
   }

   private static void CreateDefaultServiceCollection(ServiceCollection sc, IConfiguration configuration)
   {
      const string baseAddressPath = "Clients:ApiClient:BaseAddress";
      string baseAddress = configuration[baseAddressPath]
         ?? throw new InvalidOperationException($"Configuration has no base address for API client ({baseAddressPath})");

      sc.AddRefitClient<IApiClient>()
         .ConfigureHttpClient(client => client.BaseAddress = new Uri(baseAddress));

      sc.AddTransient<Mapper>();
   }

   private static IConfigurationRoot GetConfiguration()
   {
      return new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
         .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true)
         .Build();
   }
}