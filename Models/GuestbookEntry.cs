namespace Gastbok.Models;


// Representerar ett inlägg i gästboken.
// Klassen innehåller ägaren till inlägget och själva texten.

public class GuestbookEntry
{
    public string Owner { get; set; }
    public string Text { get; set; }

    public GuestbookEntry(string owner, string text)
    {
        Owner = owner;
        Text = text;
    }
}
