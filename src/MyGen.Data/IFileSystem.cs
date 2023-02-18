namespace MyGen.Data;

public interface IFileSystem
{
   Stream OpenForWrite(string filename);
   
   Stream OpenForRead(string filename);

   void DeleteFile(string filename);
   
   ICollection<string> GetFiles();
   ICollection<string> GetFiles(string extension);
}