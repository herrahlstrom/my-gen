using MyGen.Cmdlet.Models;
using MyGen.Data;
using MyGen.Data.Models;
using System.Management.Automation;

[Cmdlet(VerbsCommon.Get, "MyGenPerson")]
public class GetPersonCommand : Cmdlet
{
   [Parameter]
   public Guid Id { get; set; } = Guid.Empty;

   [Parameter]
   public string Filter { get; set; } = "";

   private CrudableRepository _repository;

   public GetPersonCommand()
   {
      var fs = new FileSystem("C:\\Users\\marti\\source\\repos\\my-gen-data");
      _repository = new CrudableRepository(fs);
      _repository.Load();
   }

   protected override void ProcessRecord()
   {
      if (Id != Guid.Empty)
      {
         var result = _repository.GetEntity<Person>(Id);
         WriteObject(PSPerson.Get(result));
      }
      else
      {
         IEnumerable<Person> result = _repository.GetEntities<Person>();

         if (!string.IsNullOrEmpty(Filter))
         {
            foreach (var word in Filter.ToLower().Split())
            {
               result = result.Where(x => x.Firstname.Contains(word, StringComparison.OrdinalIgnoreCase) ||
                                          x.Lastname.Contains(word, StringComparison.OrdinalIgnoreCase) ||
                                          x.Profession.Contains(word, StringComparison.OrdinalIgnoreCase) ||
                                          x.Notes.Contains(word, StringComparison.OrdinalIgnoreCase));
            }
         }

         WriteObject(result.Select(PSPerson.Get), true);
      }
   }
}