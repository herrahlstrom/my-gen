using MyGen.Api.Shared.Models;
using MyGen.Cmdlets.Models;

namespace MyGen.Cmdlets;
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
