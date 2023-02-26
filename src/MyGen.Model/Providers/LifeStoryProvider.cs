using MyGen.Data;
using MyGen.Data.Entities;

namespace MyGen.Model;

internal class LifeStoryProvider : EntityProvider, IEntityProvider<LifeStoryModel>
{
   public LifeStoryProvider(IEntityRepository entityRepository, EntityProviderFactory entityProviderFactory) : base(entityRepository, entityProviderFactory)
   {
   }

   public LifeStoryModel Get(Guid id)
   {
      LifeStory ls = EntityRepository.GetEntity<LifeStory>(id);
      return new LifeStoryModel(ls, null);
   }

   public LifeStoryModel Get(Guid id, LifeStoryMember lifeStoryMember)
   {
      LifeStory ls = EntityRepository.GetEntity<LifeStory>(id);
      return new LifeStoryModel(ls, lifeStoryMember);
   }
}