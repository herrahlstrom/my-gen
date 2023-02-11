using AutoBogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyGen.Api.Client;
using MyGen.Api.Shared.Models;
using MyGen.Cmdlet.Test.Helpers;
using MyGen.Cmdlets;
using MyGen.Cmdlets.Models;
using System.Management.Automation;

namespace MyGen.Cmdlet.Test;

[TestClass]
public class GetPersonCommandTest
{
   private Mock<IApiClient> _clientMock = null!;
   private Services? _services;

   [TestInitialize]
   public void Initialize()
   {
      _clientMock = new Mock<IApiClient>();
      _services = new Services((sc, config) =>
      {
         sc.AddTransient<IApiClient>(_ => _clientMock.Object);
      });
   }

   [TestCleanup]
   public void Cleanup()
   {
      _services?.Dispose();
   }

   [TestMethod]
   public void Get_ByFilter_ShouldReturnModels()
   {
      IEnumerable<PersonDto> expectedResult = new AutoFaker<PersonDto>().Generate(3);
      string filter = "abc";

      _clientMock
         .Setup(x => x.GetPersonsAsync(filter))
         .Returns(() => Task.FromResult(expectedResult));

      using var ps = PowerShell.Create();
      var result = ps.AddCommand<GetPersonCommand>()
         .AddParameter(nameof(GetPersonCommand.Filter), filter)
         .Invoke<PSPerson>();

      result.Should().BeEquivalentTo(expectedResult);

      _clientMock.Verify(e => e.GetPersonsAsync(filter), Times.Once);
   }

   [TestMethod]
   public void Get_ById_ShouldReturnModel()
   {
      PersonDto expectedResult = new AutoFaker<PersonDto>().Generate();
      Guid id = expectedResult.Id;

      _clientMock
         .Setup(x => x.GetPersonAsync(id))
         .Returns(() => Task.FromResult<PersonDto?>(expectedResult));

      using var ps = PowerShell.Create();
      var result = ps.AddCommand<GetPersonCommand>()
         .AddParameter(nameof(GetPersonCommand.Id), id)
         .Invoke<PSPerson>();

      result.Should().ContainSingle().Which.Should().BeEquivalentTo(expectedResult);

      _clientMock.Verify(e => e.GetPersonAsync(id), Times.Once);
   }

}
