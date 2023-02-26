using MyGen.Data.Entities;
using MyGen.Shared;
using MyGen.Shared.Definitions;
using MyGen.Shared.Extensions;

namespace MyGen.Model;

public class PersonModel
{
   private readonly Person _entity;
   private readonly IEnumerable<IEntityRef<FamilyModel>> _familiesAsChild;
   private readonly IEnumerable<IEntityRef<FamilyModel>> _familiesAsParent;
   private readonly Lazy<LifeStoryModel?> _lazyBirth;
   private readonly Lazy<LifeStoryModel?> _lazyDeath;

   internal PersonModel(Person entity, LifeStoryProvider lifeStoryProvider, FamilyProvider familyProvider)
   {
      _entity = entity;

      Name = new PersonName(entity.Firstname, entity.Lastname);
      Sex = Mapper.ToSex(entity.Sex);
      Profession = entity.Profession ?? "";
      Notes = entity.Notes ?? "";

      _lazyBirth = (entity.LifeStories?.Where(x => x.Type == LifeStoryType.Födelse).FirstOrDefault() is { } birthEntity)
         ? new(() => lifeStoryProvider.Get(birthEntity.LifeStoryId, birthEntity))
         : new((LifeStoryModel?)null);

      _lazyDeath = entity.LifeStories?.Where(x => x.Type == LifeStoryType.Död).FirstOrDefault() is { } deathEntity
         ? new(() => lifeStoryProvider.Get(deathEntity.LifeStoryId, deathEntity))
         : new((LifeStoryModel?)null);

      _familiesAsParent = (
         from fm in entity.Families
         where fm.MemberType.IsParent()
         select new EntityRef<FamilyModel>(fm.FamilyId, familyProvider)).ToList();

      _familiesAsChild = (
         from fm in entity.Families
         where fm.MemberType.IsChild()
         select new EntityRef<FamilyModel>(fm.FamilyId, familyProvider)).ToList();
   }

   public LifeStoryModel? Birth => _lazyBirth.Value;
   public LifeStoryModel? Death => _lazyDeath.Value;
   public IEnumerable<FamilyModel> FamiliesAsChild => _familiesAsChild.Select(x=> x.Entity);
   public IEnumerable<FamilyModel> FamiliesAsParent => _familiesAsParent.Select(x=> x.Entity);
   public Guid Id => _entity.Id;
   public PersonName Name { get; }
   public string Notes { get; }
   public string Profession { get; }
   public Sex Sex { get; }
}