using FluentAssertions;
using MyGen.Data.Models;
using MyGen.Data.Test.Services;

namespace MyGen.Data.Test;

[TestClass]
public class RepositoryTest
{
   private readonly Bogus.Faker _faker;
   private IFakeFileSystem _fileSystem = null!;
   private CrudableRepository _repository = null!;

   public RepositoryTest()
   {
      _faker = new();
   }

   [TestMethod]
   public void AddTwoThenRemoveOne_Persons_ShouldBeCorrectAmountOfFiles()
   {
      Person p1 = GetPerson();
      Person p2 = GetPerson();

      _fileSystem.Count.Should().Be(0);

      _repository.AddEntity(p1);
      _fileSystem.Count.Should().Be(0);
      _repository.Save();
      _fileSystem.Count.Should().Be(1);

      _repository.AddEntity(p2);
      _fileSystem.Count.Should().Be(1);
      _repository.Save();
      _fileSystem.Count.Should().Be(2);

      _repository.RemoveEntity(p2);
      _fileSystem.Count.Should().Be(2);
      _repository.Save();
      _fileSystem.Count.Should().Be(1);
   }

   [TestCleanup]
   public void Cleanup()
   {
      if(_fileSystem is IDisposable disposable)
      {
         disposable.Dispose();
      }
   }

   [TestMethod]
   public void Edit_Persons_ShouldBeFileModification()
   {
      Person p1 = GetPerson();
      Person p2 = GetPerson();
      
      var allChanges = _fileSystem.MonitorChanges();

      _repository.AddEntity(p1);
      _repository.AddEntity(p2);
      _repository.Save();

      var editChanges = _fileSystem.MonitorChanges();

      p2.Firstname = "Lars";

      _repository.Save();

      editChanges.Should().ContainSingle();
      allChanges.Count.Should().Be(2);
   }

   [TestInitialize]
   public void Initialize()
   {
      _fileSystem = new ShadowFileSystem();
      _repository = new CrudableRepository(_fileSystem);
   }

   private Person GetPerson()
   {
      return new Person()
      {
         Id = Guid.NewGuid(),
         Firstname = _faker.Person.FirstName,
         Lastname = _faker.Person.LastName,
         Sex = _faker.PickRandom("M", "F"),
         Profession = _faker.Person.Company.Bs,
         Notes = _faker.Random.Words(5)
      };
   }
}