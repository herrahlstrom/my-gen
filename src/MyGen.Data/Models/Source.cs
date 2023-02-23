using System.Diagnostics;

namespace MyGen.Data.Models;

[DebuggerDisplay("{Id} | {ReferenceId} | {Name} | {Url}")]
public class Source : ICrudable
{
    public Guid Id { get; set; }
    public List<Guid>? MediaIds { get; set; }
    public string? Name { get; set; }
    public string? Notes { get; set; }
    public string? Page { get; set; }
    public string? ReferenceId { get; set; }
    public string? Repository { get; set; }
    public SourceType Type { get; set; }
    public string? Url { get; set; }
    int ICrudable.Version => 1;
    public string? Volume { get; set; }

    public override int GetHashCode()
    {
        return new
        {
            Id,
            Name,
            Repository,
            Volume,
            Page,
            Url,
            ReferenceId,
            Notes,
            Type,
            MediaIds = CrudableRepository.GetHashCodeFromCollection(MediaIds)
        }.GetHashCode();
    }
}
