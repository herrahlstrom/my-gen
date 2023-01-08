namespace MyGen.Data.Test.Services;

internal interface IFakeFileSystem : IFileSystem
{
   public int Count { get; }

   #region Monitor Changes

   public IReadOnlyCollection<string> MonitorChanges();

   #endregion Monitor Changes
}

internal abstract class FakeFileSystem
{
   protected readonly HashSet<string> _files = new();
   protected List<HashSet<string>> _changedFiles = new();

   public int Count => _files.Count;
   
   public virtual void DeleteFile(string filename)
   {
      _files.Remove(filename);
   }
   public IReadOnlyCollection<string> MonitorChanges()
   {
      var changes = new HashSet<string>();
      _changedFiles.Add(changes);
      return changes;
   }
}

internal class TemporaryFileSystem : FakeFileSystem, IFakeFileSystem, IDisposable
{
   private readonly string _directory;

   public TemporaryFileSystem()
   {
      _directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
      Directory.CreateDirectory(_directory);
   }

   public Stream CreateFileStream(string filename)
   {
      var filepath = Path.Combine(_directory, filename);
      _files.Add(filename);
      _changedFiles.ForEach(f => f.Add(filename));
      return new FileStream(filepath, FileMode.Create);
   }

   public override void DeleteFile(string filename)
   {
      base.DeleteFile(filename);
      File.Delete(Path.Combine(_directory, filename));
   }

   public void Dispose()
   {
      foreach (var f in _files)
      {
         File.Delete(Path.Combine(_directory, f));
      }
      Directory.Delete(_directory);
   }

   public string[] GetFiles(string filter = "*.*")
   {
      // ToDo: Support filter

      return _files
         .Select(f => Path.Combine(_directory, f))
         .ToArray();
   }
}

internal class ShadowFileSystem : FakeFileSystem, IFakeFileSystem
{
   public Stream CreateFileStream(string filename)
   {
      _files.Add(filename);
      _changedFiles.ForEach(f => f.Add(filename));
      return Stream.Null;
   }

   public string[] GetFiles(string filter = "*.*")
   {
      return _files.ToArray();
   }
}