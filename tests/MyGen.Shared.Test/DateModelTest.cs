using FluentAssertions;
using System;

namespace MyGen.Shared.Test;

[TestClass]
public class DateModelTest
{
   [TestMethod]
   public void Year_ValidValues_ShouldSucceed()
   {
      new DateModel("1984-09-05").Year.Should().Be(1984);
      new DateModel("1984").Year.Should().Be(1984);
   }

   [TestMethod]
   public void Year_PartialValidValues_ShouldSucceed()
   {
      new DateModel("1984--05").Year.Should().Be(1984);
      new DateModel("1984-09-").Year.Should().Be(1984);
      new DateModel("1984 sep 5").Year.Should().Be(1984);
   }

   [TestMethod]
   public void Year_InvalidValues_ShouldReturnNull()
   {
      new DateModel("19840905").Year.Should().BeNull();
      new DateModel("198").Year.Should().BeNull();
      new DateModel("").Year.Should().BeNull();
   }
   
   
   [TestMethod]
   public void Date_ValidValues_ShouldSucceed()
   {
      var expected = new DateOnly(1984, 09, 05);

      new DateModel("1984-09-05").Date.Should().Be(expected);
   }
   
   [TestMethod]
   public void Date_InvalidValues_ShouldReturnNull()
   {
      new DateModel("").Date.Should().BeNull();
      new DateModel("19840905").Date.Should().BeNull();
      new DateModel("198").Date.Should().BeNull();
      new DateModel("1984--05").Date.Should().BeNull();
      new DateModel("1984-09-").Date.Should().BeNull();
      new DateModel("1984 sep 5").Date.Should().BeNull();
   }

}