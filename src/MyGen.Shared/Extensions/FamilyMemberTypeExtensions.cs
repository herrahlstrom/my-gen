using MyGen.Data.Models;

namespace MyGen.Shared.Extensions;

public static class FamilyMemberTypeExtensions
{
   public static bool IsChild(this FamilyMemberType type) => type is FamilyMemberType.Children or FamilyMemberType.FosterChildren;
}