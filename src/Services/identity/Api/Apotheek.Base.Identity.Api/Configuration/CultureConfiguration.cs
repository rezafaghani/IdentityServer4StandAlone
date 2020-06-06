﻿using System.Collections.Generic;

namespace Apotheek.Base.Identity.Api.Configuration
{
    public class CultureConfiguration
    {
        public static readonly string[] AvailableCultures = { "en", "fa", "fr", "ru", "sv", "zh", "da", "fi" };
        public static readonly string DefaultRequestCulture = "en";

        public List<string> Cultures { get; set; }
        public string DefaultCulture { get; set; } = DefaultRequestCulture;
    }
}