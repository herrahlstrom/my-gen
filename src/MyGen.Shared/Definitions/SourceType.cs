﻿using System.ComponentModel.DataAnnotations;

namespace MyGen.Shared.Definitions;

public enum SourceType
{
   None = 0,

   [Display(Name = "Annan källa")]
   Other = 1,

   [Display(Name = "Riksarkivet")]
   Riksarkivet = 2,

   [Display(Name = "Arkiv Digital")]
   ArkivDigital = 3
}