using MyGen.Shared.Definitions;

namespace MyGen.Data.Entities;

public class FamilyMember
{
   public Guid PersonId { get; set; }
   public FamilyMemberType MemberType { get; set; }

   public override int GetHashCode()
   {
      return HashCode.Combine(PersonId, MemberType);
   }
}

public class PersonFamily
{
   public Guid FamilyId { get; set; }

   public FamilyMemberType MemberType { get; set; }

   public override int GetHashCode()
   {
      return HashCode.Combine(FamilyId, MemberType);
   }
}