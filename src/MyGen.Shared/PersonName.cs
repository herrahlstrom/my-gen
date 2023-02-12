using MyGen.Shared.Extensions;

namespace MyGen.Shared;

public record PersonName
{
   public static PersonName Empty { get; } = new PersonName();

   private PersonName()
   {
      FirstName = "";
      LastName = "";

      Compositions = Array.Empty<NameComposition>();
      GivenName = "";
   }

   public PersonName(string firstName, string lastName)
   {
      FirstName = firstName;
      LastName = lastName;

      var compositions = from x in firstName.Split()
                         select x.EndsWith('*')
                           ? new NameComposition(NameCompositionType.GivenName, x[..^1])
                           : new NameComposition(NameCompositionType.FirstName, x);
      Compositions = lastName.HasValue()
         ? compositions.Append(new NameComposition(NameCompositionType.LastName, lastName)).ToArray()
         : compositions.ToArray();

      GivenName =
         Compositions.Where(x => x.Type == NameCompositionType.GivenName).Select(x => x.Value).FirstOrDefault() ??
         Compositions.Where(x => x.Type == NameCompositionType.FirstName).Select(x => x.Value).FirstOrDefault() ??
         "";
   }

   public string FirstName { get; }
   public NameComposition[] Compositions { get; }
   public string GivenName { get; }
   public string LastName { get; }
}

public enum NameCompositionType
{ FirstName, GivenName, LastName };

public record struct NameComposition(NameCompositionType Type, string Value);