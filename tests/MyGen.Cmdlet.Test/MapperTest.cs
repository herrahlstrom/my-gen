using AutoBogus;
using FluentAssertions;
using MyGen.Api.Shared.Models;
using MyGen.Cmdlets;

namespace MyGen.Cmdlet.Test;

[TestClass]
public class MapperTest
{
   Mapper _mapper = null!;

   [TestInitialize]
   public void Initialize()
   {
      _mapper = new Mapper();
   }

   [TestMethod]
   public void PersonDto_To_PSPerson()
   {
      PersonDto dto = new AutoFaker<PersonDto>().Generate();

      var psModel = _mapper.ToPsModel(dto);

      psModel.Should().BeEquivalentTo(dto);
   }
}