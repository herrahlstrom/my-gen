using Microsoft.Extensions.DependencyInjection;
using MyGen.Api.Client;
using System.Management.Automation;

namespace MyGen.Cmdlets;

[Cmdlet(VerbsCommon.Get, "MyGenMedia")]
public class GetMediaCommand : Cmdlet
{
   [Parameter(Mandatory = true, ParameterSetName = "Id")]
   public Guid Id { get; set; } = Guid.Empty;

   [Parameter(Mandatory = true, ParameterSetName = "Person")]
   public Guid PersonId { get; set; } = Guid.Empty;

   protected override void ProcessRecord()
   {
      IServiceProvider sp = Services.Instance;
      var client = sp.GetRequiredService<IApiClient>();
      var mapper = sp.GetRequiredService<Mapper>();

      if (Id != Guid.Empty)
      {
         var result = client.GetMediaAsync(Id).GetAwaiter().GetResult();
         if (result != null)
         {
            WriteObject(mapper.ToPsModel(result));
         }
      }
      else if (PersonId != Guid.Empty)
      {
         var result = client.GetMediaOnPerson(PersonId).GetAwaiter().GetResult();
         WriteObject(result.Select(mapper.ToPsModel), true);
      }
   }
}
