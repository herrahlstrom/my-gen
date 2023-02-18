using Microsoft.Extensions.Options;
using MyGen.Data;
using MyGen.Wpf.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyGen.Wpf.Tools;

public class FileSystem : IFileSystem
{
   private readonly string _path;

   public FileSystem(IOptions<DataOptions> options)
   {
      _path = options.Value.Path;

      if (string.IsNullOrWhiteSpace(_path))
      {
         throw new ArgumentException($"{nameof(DataOptions.Path)} is not defined on {nameof(DataOptions)}");
      }
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