using MyGen.Shared.Definitions;

namespace MyGen.Shared.Extensions;

public static class FamilyMemberTypeExtensions
{
   public static bool IsChild(this FamilyMemberType type) => type is FamilyMemberType.Children or FamilyMemberType.FosterChildren;
   public static bool IsParent(this FamilyMemberType type) => type is FamilyMemberType.Husband or FamilyMemberType.Wife ;
}