using MyGen.Data;

namespace MyGen.Operations;

public class FileSystem : IFileSystem
{
   private readonly string _path;

   public FileSystem(string path)
   {
      _path = path;
   }

   public void DeleteFile(string filename)
   {
      File.Delete(Path.Combine(_path, filename));
   }

   public ICollection<string> GetFiles()
   {
      return Directory.GetFiles(_path);
   }

   public ICollection<string> GetFiles(string extension)
   {
      return Directory.GetFiles(_path, $"*{extension}");
   }

   public Stream OpenForRead(string filename)
   {
      return new FileStream(Path.Combine(_path, filename), FileMode.Open);
   }

   public Stream OpenForWrite(string filename)
   {
      return new FileStream(Path.Combine(_path, filename), FileMode.Create);
   }
}