using MyGen.Data.Entities;
using MyGen.Shared;

namespace MyGen.Model;

public class LifeStoryModel
{
   private readonly LifeStory _ls;
   private readonly LifeStoryMember? _lsm;

   internal LifeStoryModel(LifeStory ls, LifeStoryMember? lsm = null)
   {
      _ls = ls;
      _lsm = lsm;

      Date = new DateModel(lsm?.Date ?? ls.Date ?? "");
      EndDate = new DateModel(lsm?.Date ?? ls.Date ?? "");
      Location = ls.Location ?? "";
   }

   public DateModel Date { get; }
   public DateModel EndDate { get; }
   public string Location { get; }
}