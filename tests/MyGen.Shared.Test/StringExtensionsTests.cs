using FluentAssertions;
using MyGen.Shared.Extensions;

namespace MyGen.Shared.Test;

[TestClass]
public class StringExtensionsTests
{
   [TestMethod]
   [DataRow("")]
   [DataRow("   ")]
   [DataRow("\t")]
   [DataRow("\r")]
   [DataRow(null)]
   public void EmptyStrings(string value)
   {
      bool result = value.HasValue();
      result.Should().BeFalse();
   }

   [TestMethod]
   [DataRow(".")]
   [DataRow("a")]
   public void NonEmptyStrings(string value)
   {
      bool result = value.HasValue();
      result.Should().BeTrue();
   }
}