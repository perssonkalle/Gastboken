using System.Text.Json;
using Gastbok.Models;

namespace Gastbok.Services;


// Ansvarar för gästbokens data och för att spara samt läsa data från JSON-fil.

public class GuestbookService
{
    private readonly string _filePath;

    // En lista med alla inlägg som finns i gästboken.
    public List<GuestbookEntry> Entries { get; private set; } = new();

    public GuestbookService(string filePath)
    {
        _filePath = filePath;
    }

    
    // För att ladda tidigare sparade inlägg från JSON-filen.
    // Om filen inte existerar så startas gästboken med en tom lista istället.
    
    public void Load()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                Entries = new List<GuestbookEntry>();
                return;
            }

            string json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                Entries = new List<GuestbookEntry>();
                return;
            }

            Entries = JsonSerializer.Deserialize<List<GuestbookEntry>>(json)
                      ?? new List<GuestbookEntry>();
        }
        catch (JsonException)
        {
            // Om JSON-filen är skadad undviker vi att programmet kraschar.
            Console.WriteLine("Kunde inte läsa gästboken eftersom JSON-filen är felaktig.");
            Entries = new List<GuestbookEntry>();
        }
        catch (IOException)
        {
            Console.WriteLine("Kunde inte läsa gästboksfilen.");
            Entries = new List<GuestbookEntry>();
        }
    }

    
    // Serialiserar listan till JSON och sparar den på disk.
    
    public void Save()
    {
        try
        {
            string? directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(Entries, options);
            File.WriteAllText(_filePath, json);
        }
        catch (IOException)
        {
            Console.WriteLine("Kunde inte spara gästboken till fil.");
        }
    }

    
    // Lägger till ett nytt inlägg och sparar direkt.
    
    public void AddEntry(string owner, string text)
    {
        Entries.Add(new GuestbookEntry(owner, text));
        Save();
    }

    
    // Tar bort ett inlägg utifrån dess index.
    
    public bool RemoveEntry(int index)
    {
        if (index < 0 || index >= Entries.Count)
        {
            return false;
        }

        Entries.RemoveAt(index);
        Save();
        return true;
    }
}
