using MyGen.Shared.Definitions;

namespace MyGen.Data.Entities;

public class FamilyMember
{
   public Guid FamilyId { get; set; }
   public Guid PersonId { get; set; }

   public FamilyMemberType MemberType { get; set; }

   public override int GetHashCode()
   {
      return HashCode.Combine(PersonId, FamilyId, MemberType);
   }
}
