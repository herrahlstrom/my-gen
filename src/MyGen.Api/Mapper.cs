using MyGen.Api.Shared.Models;
using MyGen.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MyGen.Api;

internal static class Mapper
{

   public static PersonDto ToDto(Person p)
   {
      return new PersonDto()
      {
         Id = p.Id,
         Firstname = p.Firstname,
         Lastname = p.Lastname,
         Sex = p.Sex,
         Profession = p.Profession,
         Notes = p.Notes
      };
   }
}