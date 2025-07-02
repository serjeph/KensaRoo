using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using KensaRoo.Core.Models;

namespace KensaRoo.Core.Services;

public class ProfileLibraryService
{
    public List<SteelProfile> Profiles { get; private set; } = new();

    public (bool Success, string ErrorMessage) LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return (false, $"Profile library not found: {filePath}");
        }

        try
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
            };
            string jsonString = File.ReadAllText(filePath);
            var profiles = JsonSerializer.Deserialize<List<SteelProfile>>(jsonString, options);
            
            Profiles = profiles ?? new List<SteelProfile>();
            return (true, string.Empty);
        }
        catch (JsonException jsonException)
        {
            return (false, $"Error parsing JSON file: {jsonException.Message}");
        }
        catch (Exception ex)
        {
            return (false, $"An unexpected error occurred: {ex.Message}");
        }
    }
}