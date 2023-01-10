using Microsoft.Extensions.DependencyInjection;
using MyGen.Api.Client;
using System.Management.Automation;

namespace MyGen.Cmdlets;

[Cmdlet(VerbsCommon.Get, "MyGenLifeStory")]
public class GetLifeStoryCommand : Cmdlet
{
   private IApiClient _client;

   private Mapper _mapper;

   public GetLifeStoryCommand()
   {
      IServiceProvider sp = Services.Instance;
      _client = sp.GetRequiredService<IApiClient>();
      _mapper = sp.GetRequiredService<Mapper>();
   }

   [Parameter(Mandatory = true, ParameterSetName = "Id")]
   public Guid Id { get; set; } = Guid.Empty;

   [Parameter(Mandatory = true, ParameterSetName = "Person")]
   public Guid PersonId { get; set; } = Guid.Empty;

   protected override void ProcessRecord()
   {
      if (Id != Guid.Empty)
      {
         var result = _client.GetLifeStoryAsync(Id).GetAwaiter().GetResult();
         if (result != null)
         {
            WriteObject(_mapper.ToPsModel(result));
         }
      }
      else if (PersonId != Guid.Empty)
      {
         var result = _client.GetLifeStoriesOnPerson(PersonId).GetAwaiter().GetResult();
         WriteObject(result.Select(_mapper.ToPsModel), true);
      }
   }
}
