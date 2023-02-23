using System.ComponentModel.DataAnnotations;

namespace MyGen.Data.Models;

public enum FamilyMemberType
{
   None = 0,

   [Display(Name = "Far")] Husband = 1,
   [Display(Name = "Mor")] Wife = 2,
   [Display(Name = "Barn")] Children = 3,
   [Display(Name = "Fosterbarn")] FosterChildren = 4
}