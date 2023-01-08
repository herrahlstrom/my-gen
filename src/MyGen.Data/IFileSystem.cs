namespace MyGen.Data;

public interface IFileSystem
{
   Stream CreateFileStream(string filename);

   void DeleteFile(string filename);

   string[] GetFiles(string filter = "*.*");
}