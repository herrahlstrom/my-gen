using FluentAssertions;

namespace MyGen.Shared.Test;

[TestClass]
public class MapperTests
{
   [TestMethod]
   public void ToSex_ValidValues_ShouldSucceed()
   {
      Mapper.ToSex("M").Should().Be(Sex.Male);
      Mapper.ToSex("F").Should().Be(Sex.Female);
   }

   [TestMethod]
   [DataRow("")]
   [DataRow("G")]
   [DataRow("1")]
   [DataRow("Word")]
   [DataRow(null)]
   public void ToSex_InvalidValues_ShouldReturnUnknown(string value)
   {
      Mapper.ToSex(value).Should().Be(Sex.Unknown);
   }
}
