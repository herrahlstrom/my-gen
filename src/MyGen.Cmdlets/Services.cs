using Microsoft.Extensions.DependencyInjection;
using MyGen.Api.Client;

namespace MyGen.Cmdlets;

internal class Services : IServiceProvider, IDisposable
{
   private static Services? _instance = null;
   private readonly ServiceProvider _sp;

   private Services(ServiceProvider sp)
   {
      _sp = sp;
   }

   public static Services Instance => _instance ??= Create();

   public static Services Create()
   {
      var sc = new ServiceCollection();

      sc.AddHttpClient<ApiClient>(config => { config.BaseAddress = new Uri("http://localhost:5022"); });
      sc.AddTransient<IApiClient, ApiClient>();

      sc.AddTransient<Mapper>();

      return new Services(sc.BuildServiceProvider());
   }

   public void Dispose()
   {
      _sp.Dispose();
   }

   public object? GetService(Type serviceType)
   {
      return _sp.GetService(serviceType);
   }
}