using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MyGen.Data.Models;
using MyGen.Data.Test.FileSystem;

namespace MyGen.Data.Test;

[TestClass]
public class RepositoryTest
{
   private readonly Bogus.Faker _faker;
   private IFakeFileSystem _fileSystem = null!;
   private EntityRepository _repository = null!;

   public RepositoryTest()
   {
      _faker = new();
   }

   [TestMethod]
   public void AddOne_Persons_ShouldBeCorrectAmountOfFiles()
   {
      Person p1 = GetPerson();

      _repository.AddEntity(p1);
      _repository.Save();

      VerifyFileSystem(created: 1, count: 1);
   }

   [TestMethod]
   public void AddTwo_Persons_ShouldBeCorrectAmountOfFiles()
   {
      Person p1 = GetPerson();
      Person p2 = GetPerson();

      _repository.AddEntity(p1);
      _repository.AddEntity(p2);
      _repository.Save();

      VerifyFileSystem(created: 2, count: 2);
   }

   [TestMethod]
   public void AddTwoThenRemoveOne_Persons_ShouldBeCorrectAmountOfFiles()
   {
      Person p1 = GetPerson();
      Person p2 = GetPerson();

      _repository.AddEntity(p1);
      _repository.AddEntity(p2);
      _repository.Save();

      _repository.RemoveEntity(p2);
      _repository.Save();

      VerifyFileSystem(created: 2, removed: 1, count: 1);
   }

   [TestCleanup]
   public void Cleanup()
   {
      if (_fileSystem is IDisposable disposable)
      {
         disposable.Dispose();
      }
   }

   [TestMethod]
   public void Edit_Persons_ShouldBeFileModification()
   {
      Person p1 = GetPerson();
      Person p2 = GetPerson();

      _repository.AddEntity(p1);
      _repository.AddEntity(p2);
      _repository.Save();

      p2.Firstname = "Lars";

      _repository.Save();

      VerifyFileSystem(created: 2, updated: 1, count: 2);
   }

   [TestInitialize]
   public void Initialize()
   {
      _fileSystem = new InMemoryFileSystem();
      _repository = new EntityRepository(_fileSystem, new NullLogger<EntityRepository>());
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

   private void VerifyFileSystem(int created = 0, int updated = 0, int opened = 0, int removed = 0, int count = 0)
   {
      _fileSystem.Count.Should().Be(count);

      _fileSystem.FilesCreated.Should().Be(created);
      _fileSystem.FilesUpdated.Should().Be(updated);
      _fileSystem.FilesOpened.Should().Be(opened);
      _fileSystem.FilesRemoved.Should().Be(removed);
   }
}