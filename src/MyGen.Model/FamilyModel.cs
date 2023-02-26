using MyGen.Data.Entities;
using MyGen.Shared.Definitions;
using MyGen.Shared.Extensions;

namespace MyGen.Model;

public class FamilyModel
{
   private readonly IEnumerable<IEntityRef<PersonModel>> _childMembers;
   private readonly IEntityRef<LifeStoryModel>? _divorsed;
   private readonly Family _entity;
   private readonly IEntityRef<PersonModel>? _husband;
   private readonly IEntityRef<LifeStoryModel>? _married;
   private readonly IEntityRef<PersonModel>? _wife;

   internal FamilyModel(Family entity, PersonProvider personProvider, LifeStoryProvider lifeStoryProvider)
   {
      _entity = entity;

      Notes = entity.Notes;

      _married = entity.LifeStories?.Where(x => x.Type == LifeStoryType.Gift).FirstOrDefault() is { } marriedEntity
         ? new EntityRef<LifeStoryModel>(marriedEntity.LifeStoryId, lifeStoryProvider)
         : null;

      _divorsed = entity.LifeStories?.Where(x => x.Type == LifeStoryType.Skiljd).FirstOrDefault() is { } divorsedEntity
         ? new EntityRef<LifeStoryModel>(divorsedEntity.LifeStoryId, lifeStoryProvider)
         : null;

      _childMembers = (
         from fm in entity.Members
         where fm.MemberType.IsChild()
         select new EntityRef<PersonModel>(fm.PersonId, personProvider)).ToList();

      _husband = (
         from fm in entity.Members
         where fm.MemberType == FamilyMemberType.Husband
         select new EntityRef<PersonModel>(fm.PersonId, personProvider)).SingleOrDefault();

      _wife = (
         from fm in entity.Members
         where fm.MemberType == FamilyMemberType.Wife
         select new EntityRef<PersonModel>(fm.PersonId, personProvider)).SingleOrDefault();
   }

   public IEnumerable<PersonModel> Children => _childMembers.Select(x => x.Entity);
   public LifeStoryModel? Divorsed => _divorsed?.Entity;
   public PersonModel? Husband => _husband?.Entity;
   public Guid Id => _entity.Id;
   public LifeStoryModel? Married => _married?.Entity;
   public string Notes { get; }
   public PersonModel? Wife => _wife?.Entity;
}