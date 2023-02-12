using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MyGen.Wpf.Shared;

namespace MyGen.Wpf.Main;

internal class OpenTabTemplateSelector : DataTemplateSelector
{
   public override DataTemplate SelectTemplate(object item, DependencyObject container)
   {
      var uiType = (from type in item.GetType().GetInterfaces()
                    where type.IsGenericType
                    where type.GetGenericTypeDefinition() == typeof(IViewModel<>)
                    select type.GenericTypeArguments[0]).SingleOrDefault();

      if (uiType != null)
      {
         return new DataTemplate()
         {
            VisualTree = new FrameworkElementFactory(uiType)
         };
      }

      throw new NotImplementedException($"Datatemplate for type {item.GetType()} is not implemented");
   }
}