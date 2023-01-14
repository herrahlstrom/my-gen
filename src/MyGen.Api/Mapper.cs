using MyGen.Api.Shared.Models;
using MyGen.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MyGen.Api;

internal static class Mapper
{
   public static LifeStoryDto ToDto(LifeStory lifeStory, LifeStoryMember? lifeStoryMember)
   {
      var typeDisplayAttribute = typeof(LifeStoryType)
         .GetMember(lifeStory.Type.ToString())
         .First()
         .GetCustomAttribute<DisplayAttribute>();

      return new LifeStoryDto()
      {
         Id = lifeStory.Id,
         Date = lifeStoryMember?.Date ?? lifeStory.Date ?? "",
         EndDate = lifeStoryMember?.EndDate ?? lifeStory.EndDate ?? "",
         Name = lifeStory.Name ?? "",
         Location = lifeStory.Location ?? "",
         Notes = lifeStory.Notes ?? "",
         Type = new KeyValuePair<int, string>((int)lifeStory.Type, typeDisplayAttribute?.Name ?? "")
      };
   }

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

   public static MediaDto ToDto(Media m)
   {
      return new MediaDto()
      {
         Id = m.Id,
         Title = m.Title,
         Path = m.Path,
         Size = m.Size
      };
   }
}