namespace MyGen.Data;

public class FileSystem : IFileSystem
{
   private readonly string _pwd;

   public FileSystem(string pwd)
   {
      _pwd = pwd;
   }

   public FileStream CreateFileStream(string filename)
   {
      return new FileStream(filename, FileMode.CreateNew);
   }

   public void DeleteFile(string filename)
   {
      File.Delete(filename);
   }

   public string[] GetFiles(string filter = "*.*")
   {
      return Directory.GetFiles(_pwd, filter);
   }
}