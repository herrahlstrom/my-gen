namespace MyGen.Shared;

public static class Mapper
{
   public static Sex ToSex(string value) => value switch
   {
      "M" => Sex.Male,
      "F" => Sex.Female,
      _ => Sex.Unknown
   };
}