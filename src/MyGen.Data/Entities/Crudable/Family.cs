﻿namespace MyGen.Data.Entities;

public class Family : ICrudable
{
   public Guid Id { get; init; }
   public List<PersonLifeStory>? LifeStories { get; set; }
   public string Notes { get; set; } = "";
   public IList<FamilyMember>? Members { get; set; }
   int ICrudable.Version => 1;

   public override int GetHashCode()
   {
      return new
      {
         Id,
         LifeStories = EntityRepository.GetHashCodeFromCollection(LifeStories),
         Members = EntityRepository.GetHashCodeFromCollection(Members),
         Notes
      }.GetHashCode();
   }
}