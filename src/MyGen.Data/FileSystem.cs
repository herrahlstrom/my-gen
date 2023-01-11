namespace MyGen.Data;

public class FileSystem : IFileSystem
{
   private readonly string _pwd;

   public FileSystem(string pwd)
   {
      _pwd = pwd;
   }

   public void DeleteFile(string filename)
   {
      File.Delete(Path.Combine(_pwd, filename));
   }

   public IEnumerable<string> GetFiles()
   {
      return Directory.GetFiles(_pwd);
   }

   public IEnumerable<string> GetFiles(string extension)
   {
      return Directory.GetFiles(_pwd, $"*{extension}");
   }

   public Stream OpenForRead(string filename)
   {
      return new FileStream(Path.Combine(_pwd, filename), FileMode.Open);
   }

   public Stream OpenForWrite(string filename)
   {
      return new FileStream(Path.Combine(_pwd, filename), FileMode.Create);
   }
}