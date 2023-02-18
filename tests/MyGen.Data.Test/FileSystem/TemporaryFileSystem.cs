namespace MyGen.Data.Test.FileSystem;

internal class TemporaryFileSystem : FakeFileSystem, IFakeFileSystem, IDisposable
{
   private readonly string _directory;
   private readonly HashSet<string> _files = new();

   public TemporaryFileSystem()
   {
      _directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
      Directory.CreateDirectory(_directory);
   }

   public int Count => _files.Count;

   public void DeleteFile(string filename)
   {
      _files.Remove(filename);
      File.Delete(Path.Combine(_directory, filename));
      OnFileRemoved();
   }

   public void Dispose()
   {
      foreach (var f in _files)
      {
         File.Delete(Path.Combine(_directory, f));
      }
      Directory.Delete(_directory);
   }

   public ICollection<string> GetFiles()
   {
      return _files.Select(f => Path.Combine(_directory, f)).ToList();
   }
   public ICollection<string> GetFiles(string extension)
   {
      return GetFiles().Where(x => x.EndsWith(extension, StringComparison.OrdinalIgnoreCase)).ToList();
   }

   public Stream OpenForRead(string filename)
   {
      var filepath = Path.Combine(_directory, filename);
      _files.Add(filename);

      OnFileOpened();

      return new FileStream(filepath, FileMode.Open);
   }

   public Stream OpenForWrite(string filename)
   {
      if (_files.Contains(filename))
      {
         OnFileUpdated();
      }
      else
      {
         _files.Add(filename);
         OnFileCreated();
      }

      var filepath = Path.Combine(_directory, filename);
      return new FileStream(filepath, FileMode.Create);
   }
}