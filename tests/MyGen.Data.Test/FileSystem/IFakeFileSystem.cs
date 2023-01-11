namespace MyGen.Data.Test.FileSystem;

internal interface IFakeFileSystem : IFileSystem
{
   public event EventHandler FileCreated;

   public event EventHandler FileOpened;

   public event EventHandler FileRemoved;

   public event EventHandler FileUpdated;

   public int Count { get; }

   public int FilesCreated { get; }
   public int FilesOpened { get; }
   public int FilesRemoved { get; }
   public int FilesUpdated { get; }
}
