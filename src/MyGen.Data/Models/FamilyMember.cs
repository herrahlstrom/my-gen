using System.ComponentModel.DataAnnotations;

namespace MyGen.Data.Models;

public class FamilyMember
{
   public Guid FamilyId { get; set; }

   public FamilyMemberType MemberType { get; set; }
   
   public override int GetHashCode()
   {
      return HashCode.Combine(FamilyId, MemberType);
   }
}

public enum FamilyMemberType
{
   None = 0,

   [Display(Name = "Far")] Husband = 1,
   [Display(Name = "Mor")] Wife = 2,
   [Display(Name = "Barn")] Children = 3,
   [Display(Name = "Fosterbarn")] FosterChildren = 4
}