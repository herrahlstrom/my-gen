using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618

namespace MyGen.Data.EF.Models;

public class LifeStoryType
{
    [Column("id")]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("name")]
    public string Name { get; set; }
}