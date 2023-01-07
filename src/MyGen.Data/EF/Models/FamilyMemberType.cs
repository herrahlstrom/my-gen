using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618

namespace MyGen.Data.EF.Models;

public class FamilyMemberType
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [MaxLength(30)]
    public string Name { get; set; }
}