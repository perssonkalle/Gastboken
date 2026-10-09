using Gastbok.Services;

namespace Gastbok;

internal class Program
{
    // JSON-filen där gästbokens inlägg sparas.
    private static readonly string FilePath =
        Path.Combine(AppContext.BaseDirectory, "data", "guestbook.json");

    static void Main()
    {
        var guestbook = new GuestbookService(FilePath);

        // Läser in tidigare sparade inlägg när programmet startar.
        guestbook.Load();

        bool running = true;

        while (running)
        {
            Console.Clear();
            ShowGuestbook(guestbook);

            Console.WriteLine();
            Console.WriteLine("1. Skriv i gästboken");
            Console.WriteLine("2. Ta bort inlägg");
            Console.WriteLine();
            Console.WriteLine("X. Avsluta");
            Console.WriteLine();

            Console.Write("Välj ett alternativ: ");
            string choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice.ToUpper())
            {
                case "1":
                    AddEntry(guestbook);
                    break;

                case "2":
                    RemoveEntry(guestbook);
                    break;

                case "X":
                    running = false;
                    break;

                default:
                    Console.WriteLine();
                    Console.WriteLine("Felaktigt menyval. Välj 1, 2 eller X.");
                    Pause();
                    break;
            }
        }

        Console.Clear();
        Console.WriteLine("Tack för att du använde gästboken!");
    }

    
    // Gör så att den skriver ut alla inlägg med index till vänster.
    
    private static void ShowGuestbook(GuestbookService guestbook)
    {
        Console.WriteLine("K A L L E S ' G U E S T B O O K");
        Console.WriteLine(new string('=', 34));
        Console.WriteLine();

        if (guestbook.Entries.Count == 0)
        {
            Console.WriteLine("Gästboken är tom.");
            return;
        }

        for (int i = 0; i < guestbook.Entries.Count; i++)
        {
            var entry = guestbook.Entries[i];

            Console.WriteLine($"[{i}] {entry.Owner} - {entry.Text}");
        }
    }

    
    // Ägare och text läses in och kontrollerar att båda fälten innehåller data.
    
    private static void AddEntry(GuestbookService guestbook)
    {
        Console.Clear();
        Console.WriteLine("SKRIV I GÄSTBOKEN");
        Console.WriteLine();

        string owner = ReadRequired("Ägare: ");
        string text = ReadRequired("Inlägg: ");

        guestbook.AddEntry(owner, text);

        Console.WriteLine();
        Console.WriteLine("Inlägget har lagts till!");
        Pause();
    }

    
    // Frågar efter index och tar bort det valda inlägget.
    
    private static void RemoveEntry(GuestbookService guestbook)
    {
        Console.Clear();
        Console.WriteLine("TA BORT INLÄGG");
        Console.WriteLine();

        if (guestbook.Entries.Count == 0)
        {
            Console.WriteLine("Det finns inga inlägg att ta bort.");
            Pause();
            return;
        }

        ShowGuestbook(guestbook);
        Console.WriteLine();

        Console.Write("Ange index på inlägget som ska tas bort: ");
        string input = Console.ReadLine()?.Trim() ?? "";

        // Använder TryParse för att undvika att programmet ska krascha
        // om användaren skriver exempelvis "abc" i stället för ett tal.
        if (!int.TryParse(input, out int index))
        {
            Console.WriteLine("Fel: du måste ange ett heltal.");
            Pause();
            return;
        }

        if (!guestbook.RemoveEntry(index))
        {
            Console.WriteLine("Fel: indexet finns inte i gästboken.");
            Pause();
            return;
        }

        Console.WriteLine("Inlägget har tagits bort!");
        Pause();
    }

    
    // Läser en text från användaren och fortsätter att fråga tills fältet inte är tomt längre.
    
    private static string ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim() ?? "";

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            Console.WriteLine("Fältet får inte vara tomt. Försök igen.");
        }
    }

    
    // Programmet pausas så att användaren kan läsa meddelandet
    
    private static void Pause()
    {
        Console.WriteLine();
        Console.Write("Tryck Enter för att fortsätta...");
        Console.ReadLine();
    }
}
