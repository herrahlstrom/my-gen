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
