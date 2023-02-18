namespace MyGen.Data.Test.FileSystem;

internal class InMemoryFileSystem : FakeFileSystem, IFakeFileSystem
{
   private readonly SemaphoreSlim m_lock = new(1);
   private readonly Dictionary<string, IEntry> _files = new(StringComparer.OrdinalIgnoreCase);

   private interface IEntry
   {
      public string Name { get; }
   }

   public int Count => _files.Count;

   public void CreateDirectory(string directoryPath)
   {
      lock (m_lock)
      {
         _files[directoryPath] = new DirectoryEntry(directoryPath);
      }
   }

   public void DeleteFile(string filename)
   {
      lock (m_lock)
      {
         _files.Remove(filename);
         OnFileRemoved();
      }
   }

   public bool DirectoryExists(string directoryPath)
   {
      lock (m_lock)
      {
         if (_files.TryGetValue(directoryPath, out var entry) && entry is DirectoryEntry)
         {
            return true;
         }

         return _files.Any(item => item.Key.StartsWith(directoryPath, StringComparison.OrdinalIgnoreCase));
      }
   }

   public bool FileExists(string filePath)
   {
      return _files.TryGetValue(filePath, out var entry) && entry is FileEntry;
   }

   public ICollection<string> GetFiles()
   {
      return _files.Values.Select(x => x.Name).ToList();
   }

   public ICollection<string> GetFiles(string extension)
   {
      return GetFiles().Where(x => x.EndsWith(extension, StringComparison.OrdinalIgnoreCase)).ToList();
   }

   public Stream OpenForRead(string path)
   {
      lock (m_lock)
      {
         if (_files[path] is FileEntry file)
         {
            OnFileOpened();
            return new LockingMemoryStream(file.Contents);
         }
      }

      throw new FileNotFoundException();
   }

   public Stream OpenForWrite(string path)
   {
      m_lock.Wait();
      try
      {
         if (_files.TryGetValue(path, out var entry) && entry is FileEntry file)
         {
            var stream = new LockingMemoryStream(data => file.Contents = data);

            if (file.Contents is { Length: > 0 } contents)
            {
               stream.Write(contents, 0, contents.Length);
               stream.Position = 0;
            }

            OnFileUpdated();

            return stream;
         }
         else
         {
            file = new FileEntry(path, Array.Empty<byte>());
            _files[path] = file;

            OnFileCreated();

            return new LockingMemoryStream(data => file.Contents = data);
         }
      }
      finally
      {
         m_lock.Release();
      }
   }
   private class DirectoryEntry : IEntry
   {
      public DirectoryEntry(string name)
      {
         Name = name;
      }

      public string Name { get; }
   }

   private class FileEntry : IEntry
   {
      public FileEntry(string name, byte[] contents)
      {
         Name = name;
         Contents = contents;
      }

      public byte[] Contents { get; set; }
      public string Name { get; }
   }

   private class LockingMemoryStream : MemoryStream
   {
      private readonly Action<byte[]>? _closeCallback;
      private int _closed = 0;

      public LockingMemoryStream(Action<byte[]> closeCallback)
      {
         _closeCallback = closeCallback;
      }

      public LockingMemoryStream(byte[] data) : base(data, false)
      {
         _closeCallback = null;
      }

      public override void Close()
      {
         if (Interlocked.Exchange(ref _closed, 1) == 0)
         {
            _closeCallback?.Invoke(base.ToArray());
         }

         base.Close();
      }
   }
}