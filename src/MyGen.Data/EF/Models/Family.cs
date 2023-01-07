using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618

namespace MyGen.Data.EF.Models;

public class Family
{
    [Column("id")]
    public int Id { get; set; }

    public ICollection<FamilyLifeStory> LifeStories { get; set; }

    public ICollection<FamilyMember> Members { get; set; }

    [Column("notes")]
    public string Notes { get; set; } = "";
}