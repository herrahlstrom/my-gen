using Microsoft.Extensions.DependencyInjection;
using MyGen.Api.Client;
using System.Management.Automation;

namespace MyGen.Cmdlets;

[Cmdlet(VerbsCommon.Get, "MyGenPerson")]
public class GetPersonCommand : Cmdlet
{
   [Parameter]
   public string Filter { get; set; } = "";

   [Parameter]
   public Guid Id { get; set; } = Guid.Empty;

   protected override void ProcessRecord()
   {
      IServiceProvider sp = Services.Instance;
      var client = sp.GetRequiredService<IApiClient>();
      var mapper = sp.GetRequiredService<Mapper>();

      if (Id != Guid.Empty)
      {
         var result = client.GetPersonAsync(Id).GetAwaiter().GetResult();
         if (result != null)
         {
            WriteObject(mapper.ToPsModel(result));
         }
      }
      else
      {
         var result = client.GetPersonsAsync(Filter).GetAwaiter().GetResult();
         WriteObject(result.Select(mapper.ToPsModel), true);
      }
   }
}
