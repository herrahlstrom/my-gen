using MyGen.Shared.Extensions;
using System.Text.RegularExpressions;

namespace MyGen.Shared;

public partial class DateModel : IEquatable<DateModel>
{
   private readonly Lazy<DateOnly?> _lazyDate;
   private readonly Lazy<int?> _lazyYear;

   public DateModel(string value)
   {
      Value = value;
      _lazyYear = new Lazy<int?>(GetYear);
      _lazyDate = new Lazy<DateOnly?>(GetDate);
   }

   public static DateModel Empty { get; } = new DateModel("");
   public DateOnly? Date => _lazyDate.Value;

   public string DisplayDate
   {
      get
      {
         if (Date is { } d)
         {
            return d.ToString("d MMM yyyy");
         }
         if (Year is { } y)
         {
            return y.ToString();
         }
         return Value;
      }
   }

   public bool HasValue => Value.HasValue();
   public string Value { get; }
   public int? Year => _lazyYear.Value;

   public bool Equals(DateModel? other)
   {
      return other != null && other.Value == Value;
   }

   [GeneratedRegex(@"^(?<year>\d{4})\-(?<month>\d{2})\-(?<day>\d{2})$")]
   private static partial Regex ExactDateRegex();

   [GeneratedRegex(@"^(?<year>\d{4})(\D|$)")]
   private static partial Regex YearRegex();

   private DateOnly? GetDate()
   {
      if (ExactDateRegex().Match(Value) is { Success: true } m)
      {
         return new DateOnly(
            int.Parse(m.Groups["year"].Value),
            int.Parse(m.Groups["month"].Value),
            int.Parse(m.Groups["day"].Value));
      }
      return null;
   }

   private int? GetYear()
   {
      if (YearRegex().Match(Value) is { Success: true } m)
      {
         return int.Parse(m.Groups["year"].Value);
      }
      return null;
   }
}