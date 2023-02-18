using System;

namespace MyGen.Wpf.Tools;

internal class AppState
{
   public AppCallback<Guid> OpenPerson { get; } = new AppCallback<Guid>();

   public class AppCallback<TParameter>
   {
      private Action<TParameter>? _callback;

      public void Request(TParameter parameter) => _callback?.Invoke(parameter);

      public void SetRequestCallback(Action<TParameter> callback) => _callback = callback;
   }
}