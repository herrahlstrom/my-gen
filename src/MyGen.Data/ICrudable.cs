namespace MyGen.Data;

public interface ICrudable
{
    Guid Id { get; }

    int Version { get; }
}
