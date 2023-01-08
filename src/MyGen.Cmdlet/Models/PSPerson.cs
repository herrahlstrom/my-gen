using MyGen.Data.Models;

namespace MyGen.Cmdlet.Models;

public class PSPerson
{
   public required Guid Id { get; set; }
   public required string Firstname { get; set; }
   public required string Lastname { get; set; }
   public required string Sex { get; set; }
   public required string Profession { get; set; }
   public required string Notes { get; set; }

   internal static PSPerson Get(Person entity)
   {
      return new PSPerson()
      {
         Id = entity.Id,
         Firstname = entity.Firstname,
         Lastname = entity.Lastname,
         Sex = entity.Sex,
         Profession = entity.Profession,
         Notes = entity.Notes
      };
   }
}