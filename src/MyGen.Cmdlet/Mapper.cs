using MyGen.Api.Shared.Models;
using MyGen.Cmdlet.Models;

namespace MyGen.Cmdlet;
internal class Mapper
{
   public PSPerson ToPsModel(PersonDto dto)
   {
      return new PSPerson()
      {
         Id = dto.Id,
         Firstname = dto.Firstname,
         Lastname = dto.Lastname,
         Sex = dto.Sex,
         Profession = dto.Profession,
         Notes = dto.Notes
      };
   }
}
