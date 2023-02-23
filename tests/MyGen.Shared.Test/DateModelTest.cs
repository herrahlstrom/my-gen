using FluentAssertions;
using System;

namespace MyGen.Shared.Test;

[TestClass]
public class DateModelTest
{
   [TestMethod]
   public void Year_ValidValues_ShouldSucceed()
   {
      new EventDate("1984-09-05").Year.Should().Be(1984);
      new EventDate("1984").Year.Should().Be(1984);
   }

   [TestMethod]
   public void Year_PartialValidValues_ShouldSucceed()
   {
      new EventDate("1984--05").Year.Should().Be(1984);
      new EventDate("1984-09-").Year.Should().Be(1984);
      new EventDate("1984 sep 5").Year.Should().Be(1984);
   }

   [TestMethod]
   public void Year_InvalidValues_ShouldReturnNull()
   {
      new EventDate("19840905").Year.Should().BeNull();
      new EventDate("198").Year.Should().BeNull();
      new EventDate("").Year.Should().BeNull();
   }
   
   
   [TestMethod]
   public void Date_ValidValues_ShouldSucceed()
   {
      var expected = new DateOnly(1984, 09, 05);

      new EventDate("1984-09-05").Date.Should().Be(expected);
   }
   
   [TestMethod]
   public void Date_InvalidValues_ShouldReturnNull()
   {
      new EventDate("").Date.Should().BeNull();
      new EventDate("19840905").Date.Should().BeNull();
      new EventDate("198").Date.Should().BeNull();
      new EventDate("1984--05").Date.Should().BeNull();
      new EventDate("1984-09-").Date.Should().BeNull();
      new EventDate("1984 sep 5").Date.Should().BeNull();
   }

}