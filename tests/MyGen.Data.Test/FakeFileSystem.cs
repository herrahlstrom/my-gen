namespace MyGen.Data.Test;

internal class FakeFileSystem : IFileSystem, IDisposable
{
   private readonly string _directory;
   private readonly List<string> _files = new();

   public FakeFileSystem()
   {
      _directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
      Directory.CreateDirectory(_directory);
   }

   public event EventHandler<string> FileChanged;

   public int Count => _files.Count;

   public FileStream CreateFileStream(string filename)
   {
      var filepath = Path.Combine(_directory, filename);
      _files.Add(filename);
      FileStream fileStream = new FileStream(filepath, FileMode.Create);
      OnFileChanged(filename);
      return fileStream;
   }

   public void DeleteFile(string filename)
   {
      File.Delete(Path.Combine(_directory, filename));
      _files.Remove(filename);
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
      string[] result = new string[_files.Count];
      for (int i = 0; i < _files.Count; i++)
      {
         result[i] = Path.Combine(_directory, _files[i]);
      }
      return result;
   }

   protected virtual void OnFileChanged(string e)
   {
      FileChanged?.Invoke(this, e);
   }
}