using FluentAssertions;
using System.Linq;

namespace MyGen.Shared.Test;

[TestClass]
public class PersonNameTests
{
   [TestMethod]
   [DataRow("Adam Bertil* Ceasar", "Davidsson", "Bertil")]
   [DataRow("Adam Bertil Ceasar", "Davidsson", "Adam")]
   public void GivenName(string firstName, string lastName, string expectedGivenName)
   {
      var sut = new PersonName(firstName, lastName);

      sut.GivenName.Should().Be(expectedGivenName);
   }

   [TestMethod]
   public void NameCompositionType_Konrad()
   {
      var sut = new PersonName("Adam Bertil Ceasar", "Davidsson");

      sut.Compositions.Select(x => x.Type).Should().BeEquivalentTo(new[] {
         NameCompositionType.FirstName,
         NameCompositionType.FirstName,
         NameCompositionType.FirstName,
         NameCompositionType.LastName });
   }

   [TestMethod]
   public void NameCompositionType_Martin()
   {
      var sut = new PersonName("Adam Bertil* Ceasar", "Davidsson");

      sut.Compositions.Select(x => x.Type).Should().BeEquivalentTo(new[] {
         NameCompositionType.FirstName,
         NameCompositionType.GivenName,
         NameCompositionType.FirstName,
         NameCompositionType.LastName });
   }
}