using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace MyGen.Data.Models;

public class Media : ICrudable
{
   public Guid Id { get; set; }

   public string Path { get; set; } = null!;

   int ICrudable.Version => 1;

   public override int GetHashCode()
   {
      return HashCode.Combine(Id, Path);
   }
}