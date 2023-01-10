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
public class GetLifeStoryCommandTest
{
   private Mock<IApiClient> _clientMock = null!;
   private Services? _services;

   [TestInitialize]
   public void Initialize()
   {
      _clientMock = new Mock<IApiClient>();
      _services = new Services(sc =>
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
   public void Get_ByPerson_ShouldReturnModels()
   {
      Guid personId = Guid.NewGuid();
      IEnumerable<LifeStoryDto> expectedResult = new AutoFaker<LifeStoryDto>().Generate(3);

      _clientMock
         .Setup(x => x.GetLifeStoriesOnPerson(personId))
         .Returns(() => Task.FromResult(expectedResult));

      using var ps = PowerShell.Create();
      var result = ps.AddCommand<GetLifeStoryCommand>()
         .AddParameter(nameof(GetLifeStoryCommand.PersonId), personId)
         .Invoke<PSLifeStory>();

      result.Should().BeEquivalentTo(expectedResult);

      _clientMock.Verify(e => e.GetLifeStoriesOnPerson(personId), Times.Once);
   }

   [TestMethod]
   public void Get_ById_ShouldReturnModel()
   {
      Guid id = Guid.NewGuid();
      LifeStoryDto expectedResult = new AutoFaker<LifeStoryDto>().Generate();

      _clientMock
         .Setup(x => x.GetLifeStoryAsync(id))
         .Returns(() => Task.FromResult(expectedResult));

      using var ps = PowerShell.Create();
      var result = ps.AddCommand<GetLifeStoryCommand>()
         .AddParameter(nameof(GetLifeStoryCommand.Id), id)
         .Invoke<PSLifeStory>();

      result.Should().ContainSingle().Which.Should().BeEquivalentTo(expectedResult);

      _clientMock.Verify(e => e.GetLifeStoryAsync(id), Times.Once);
   }

}
