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

   public PSLifeStory ToPsModel(LifeStoryDto dto)
   {
      return new PSLifeStory()
      {
         Id = dto.Id,
         Name = dto.Name,
         Date = dto.Date,
         EndDate = dto.EndDate,
         Location = dto.Location,
         Type = dto.Type,
         Notes = dto.Notes
      };
   }
}
