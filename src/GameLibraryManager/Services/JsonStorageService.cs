using System.Text.Json;
using GameLibraryManager.Models;

namespace GameLibraryManager.Services;

public class JsonStorageService
{
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonStorageService()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }

    public bool SavePlayers(string filePath, List<Player> players, out string message)
    {
        try
        {
            string? directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonSerializer.Serialize(players, _jsonOptions);
            File.WriteAllText(filePath, json);
            message = "Data saved successfully.";
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            message = "The application does not have permission to save the file.";
            return false;
        }
        catch (IOException ex)
        {
            message = $"A file error occurred while saving data: {ex.Message}";
            return false;
        }
        catch (Exception ex)
        {
            message = $"Could not save data: {ex.Message}";
            return false;
        }
    }

    public bool LoadPlayers(string filePath, out List<Player> players, out string message)
    {
        players = new List<Player>();

        try
        {
            if (!File.Exists(filePath))
            {
                message = "Data file was not found.";
                return false;
            }

            string json = File.ReadAllText(filePath);
            List<Player>? loadedPlayers = JsonSerializer.Deserialize<List<Player>>(json, _jsonOptions);

            if (loadedPlayers == null)
            {
                message = "No player data was found in the file.";
                return false;
            }

            if (HasDuplicatePlayerIds(loadedPlayers))
            {
                message = "The JSON file contains duplicate player IDs.";
                return false;
            }

            players = loadedPlayers;
            message = "Data loaded successfully.";
            return true;
        }
        catch (JsonException)
        {
            message = "The JSON file is not in a valid format.";
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            message = "The application does not have permission to load the file.";
            return false;
        }
        catch (IOException ex)
        {
            message = $"A file error occurred while loading data: {ex.Message}";
            return false;
        }
        catch (Exception ex)
        {
            message = $"Could not load data: {ex.Message}";
            return false;
        }
    }

    private bool HasDuplicatePlayerIds(List<Player> players)
    {
        return players
            .GroupBy(player => player.PlayerId)
            .Any(group => group.Count() > 1);
    }
}
