#r "Newtonsoft.Json"

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class JsonConfiguration
{
    public static void Configure()
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        };

        settings.Converters.Add(new StringEnumConverter { CamelCaseText = false });

        JsonConvert.DefaultSettings = (() => settings);
    }
}

