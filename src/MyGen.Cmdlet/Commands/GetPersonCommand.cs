using Microsoft.Extensions.DependencyInjection;
using MyGen.Api.Client;
using MyGen.Cmdlet;
using System.Management.Automation;

[Cmdlet(VerbsCommon.Get, "MyGenPerson")]
public class GetPersonCommand : Cmdlet
{
   [Parameter]
   public Guid Id { get; set; } = Guid.Empty;

   [Parameter]
   public string Filter { get; set; } = "";

   private IApiClient _client;
   private Mapper _mapper;

   public GetPersonCommand()
   {
      IServiceProvider sp = Services.Instance;
      _client = sp.GetRequiredService<IApiClient>();
      _mapper = sp.GetRequiredService<Mapper>();
   }

   protected override void ProcessRecord()
   {
      if (Id != Guid.Empty)
      {
         var result = _client.GetPersonAsync(Id).GetAwaiter().GetResult();
         if (result != null)
         {
            WriteObject(_mapper.ToPsModel(result));
         }
      }
      else
      {
         var result = _client.GetPersonsAsync(Filter).GetAwaiter().GetResult();
         WriteObject(result.Select(_mapper.ToPsModel), true);
      }
   }
}