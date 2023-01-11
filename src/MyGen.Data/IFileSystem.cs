namespace MyGen.Data;

public interface IFileSystem
{
   Stream OpenForWrite(string filename);
   
   Stream OpenForRead(string filename);

   void DeleteFile(string filename);
   
   IEnumerable<string> GetFiles();
   IEnumerable<string> GetFiles(string extension);
}