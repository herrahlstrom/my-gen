namespace MyGen.Data;

public interface IFileSystem
{
   FileStream CreateFileStream(string filename);

   void DeleteFile(string filename);

   string[] GetFiles(string filter = "*.*");
}