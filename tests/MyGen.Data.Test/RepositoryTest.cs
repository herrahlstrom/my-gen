using MyGen.Data.Models;

namespace MyGen.Data.Test;

[TestClass]
public class RepositoryTest
{
   private Bogus.Faker _faker;
   private FakeFileSystem _fileSystem = null!;
   private CrudableRepository _repository = null!;

   public RepositoryTest()
   {
      _faker = new();
   }

   [TestMethod]
   public void AddTwoThenRemoveOne_Persons_ShouldBeCorrectAmountOfFiles()
   {
      Person martin = new Person()
      {
         Id = Guid.NewGuid(),
         Firstname = "Martin",
         Lastname = "Ahlström",
         Sex = "M"
      };

      Person anders = new Person()
      {
         Id = Guid.NewGuid(),
         Firstname = "Anders",
         Lastname = "Andersson",
         Sex = "M"
      };

      _fileSystem.Count.Should().Be(0);

      _repository.AddEntity(martin);
      _fileSystem.Count.Should().Be(0);
      _repository.Save();
      _fileSystem.Count.Should().Be(1);

      _repository.AddEntity(anders);
      _fileSystem.Count.Should().Be(1);
      _repository.Save();
      _fileSystem.Count.Should().Be(2);

      _repository.RemoveEntity(anders);
      _fileSystem.Count.Should().Be(2);
      _repository.Save();
      _fileSystem.Count.Should().Be(1);
   }

   [TestCleanup]
   public void Cleanup()
   {
      _fileSystem.Dispose();
   }

   [TestMethod]
   public void Edit_Persons_ShouldBeFileModification()
   {
      Person p1 = GetPerson();
      Person p2 = GetPerson();

      _repository.AddEntity(p1);
      _repository.AddEntity(p2);
      _repository.Save();

      HashSet<string> modifiedFiles = new();
      _fileSystem.FileChanged += (_, e) => modifiedFiles.Add(e);

      p2.Firstname = "Lars";

      _repository.Save();

      modifiedFiles.Should().ContainSingle();
   }

   [TestInitialize]
   public void Initialize()
   {
      _fileSystem = new FakeFileSystem();
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