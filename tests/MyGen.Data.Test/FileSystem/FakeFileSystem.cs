namespace MyGen.Data.Test.FileSystem;

internal abstract class FakeFileSystem
{
   protected FakeFileSystem()
   {
      FileCreated += (_, _) => FilesCreated++;
      FileOpened += (_, _) => FilesOpened++;
      FileUpdated += (_, _) => FilesUpdated++;
      FileRemoved += (_, _) => FilesRemoved++;
   }

   public event EventHandler? FileCreated;

   public event EventHandler? FileOpened;

   public event EventHandler? FileRemoved;

   public event EventHandler? FileUpdated;

   public int FilesCreated { get; private set; }
   public int FilesOpened { get; private set; }
   public int FilesRemoved { get; private set; }
   public int FilesUpdated { get; private set; }

   public void ResetFilesCounter()
   {
      FilesCreated = 0;
      FilesOpened = 0;
      FilesUpdated = 0;
      FilesRemoved = 0;
   }

   protected void OnFileCreated() => FileCreated?.Invoke(this, EventArgs.Empty);

   protected void OnFileOpened() => FileOpened?.Invoke(this, EventArgs.Empty);

   protected void OnFileRemoved() => FileRemoved?.Invoke(this, EventArgs.Empty);

   protected void OnFileUpdated() => FileUpdated?.Invoke(this, EventArgs.Empty);
}