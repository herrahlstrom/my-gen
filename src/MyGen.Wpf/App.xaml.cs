﻿using Microsoft.Extensions.DependencyInjection;
using MyGen.Data;
using MyGen.Model;
using MyGen.Wpf.Main;
using MyGen.Wpf.Shared;
using System;
using System.Windows;

namespace MyGen.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application, IDisposable
{
   private readonly Services _services;

   public App()
   {
      _services = Services.Create();

      Startup += OnStartup;
      Exit += OnExit;

      ShutdownMode = ShutdownMode.OnMainWindowClose;
   }

   public void Dispose()
   {
      _services?.Dispose();
   }

   private void OnExit(object sender, ExitEventArgs e)
   {
      Dispose();
   }

   private void OnStartup(object sender, StartupEventArgs e)
   {
      _services.GetRequiredService<IEntityRepository>().Load();

      ViewModelFactory vmFactory = _services.GetRequiredService<ViewModelFactory>();
      IViewModel<MainWindow> mainVm = vmFactory.CreateViewModelFor<MainWindow>();

      mainVm.Load();

      MainWindow = new MainWindow()
      {
         DataContext = mainVm
      };

      MainWindow.Show();
   }
}