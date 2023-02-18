using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyGen.Data;
using MyGen.Wpf.Options;
using MyGen.Wpf.Shared;
using MyGen.Wpf.Tools;
using System;
using System.IO;
using System.Windows.Controls;

namespace MyGen.Wpf;

internal static class ServiceProviderExtensions
{
   public static ServiceCollection RegisterViewAndModel<TView, TModel, TRepository>(this ServiceCollection sc)
      where TView : ContentControl
      where TModel : class, IViewModel<TView>
      where TRepository : class, IViewModelRepository<TModel>
   {
      // View Model and Factory
      sc.AddTransient<TModel>();
      sc.AddTransient<IViewModel<TView>>(sp => sp.GetRequiredService<TModel>());

      // View Model Repository
      sc.AddSingleton<IViewModelRepository<TModel>, TRepository>();

      return sc;
   }
}

internal class Services : IServiceProvider, IDisposable
{
   private readonly ServiceProvider _serviceProvider;

   public Services(Action<ServiceCollection, IConfiguration> scBuilder)
   {
      IConfigurationRoot configuration = GetConfiguration();

      var sc = new ServiceCollection();
      scBuilder.Invoke(sc, configuration);
      _serviceProvider = sc.BuildServiceProvider();
   }

   private Services() : this(CreateDefaultServiceCollection)
   {
   }

   public static Services Create()
   {
      return new Services(CreateDefaultServiceCollection);
   }

   public void Dispose()
   {
      _serviceProvider.Dispose();
   }

   public object? GetService(Type serviceType)
   {
      return _serviceProvider.GetService(serviceType);
   }

   private static void CreateDefaultServiceCollection(ServiceCollection sc, IConfiguration configuration)
   {
      sc.AddSingleton<AppState>();

      sc.Configure<DataOptions>((options) => configuration.GetSection("Data").Bind(options));
      sc.AddSingleton<IFileSystem, Tools.FileSystem>();
      sc.AddSingleton<MyGen.Data.CrudableRepository>();

      sc.AddLogging(configure => configure
         .AddConfiguration(configuration.GetRequiredSection("Logging"))
         .AddDebug());

      sc.RegisterViewAndModel<Main.MainWindow, Main.MainViewModel, Main.MainViewModelRepository>();
      sc.RegisterViewAndModel<Main.SearchUserControl, Main.SearchViewModel, Main.SearchViewModelRepository>();
      sc.RegisterViewAndModel<Person.PersonUserControl, Person.PersonViewModel, Person.PersonViewModelRepository>();

      sc.AddSingleton<ViewModelFactory>();
   }

   private static IConfigurationRoot GetConfiguration()
   {
      return new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
         .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true)
         .AddUserSecrets<Services>(optional: false)
         .Build();
   }
}