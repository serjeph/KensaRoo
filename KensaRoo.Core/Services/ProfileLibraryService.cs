using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using KensaRoo.Core.Models;

namespace KensaRoo.Core.Services;

public class ProfileLibraryService
{
    public List<SteelProfile> Profiles { get; set; } = new();

    public async Task<(bool Success, string ErrorMessage)> LoadFromFileAsync(string filePath)
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
            await using var stream = File.OpenRead(filePath);
            var profiles = await JsonSerializer.DeserializeAsync<List<SteelProfile>>(stream, options);
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