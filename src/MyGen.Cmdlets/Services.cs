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

   public Services(Action<ServiceCollection> scBuilder)
   {
      var sc = new ServiceCollection();
      scBuilder.Invoke(sc);
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

   private static void CreateDefaultServiceCollection(ServiceCollection sc)
   {
      sc.AddRefitClient<IApiClient>()
         .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://localhost:5022"));

      sc.AddTransient<Mapper>();
   }
}