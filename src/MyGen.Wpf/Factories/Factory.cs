using System;

namespace MyGen.Wpf.Infrastructure;

internal interface IFactory<T>
{
   T Create();
}

internal class SimpleFactory<T> : IFactory<T>
{
   private readonly Func<T> _factory;

   public SimpleFactory(Func<T> factory)
   {
      _factory = factory;
   }

   public T Create() => _factory.Invoke();
}