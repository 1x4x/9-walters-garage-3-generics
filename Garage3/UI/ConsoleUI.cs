using Garage3.Handlers;
using Garage3.Interfaces;
using Garage3.Models;

namespace Garage3.UI;

/// <summary>
/// Konsolbaserat användargränssnitt. All in- och utmatning sker här.
/// </summary>
public sealed class ConsoleUI : IUI
{
    private IHandler? _handler;

    public void Run()
    {
        Console.WriteLine("--- WALTERS GARAGE 3.0 ---");
        int capacity = ReadInt("Ange garagekapacitet: ", 1, 3000);
        _handler = new GarageHandler(capacity, CreateSampleVehicles().Take(capacity).ToArray());

        bool running = true;
        while (running)
        {
            ShowMainMenu();
            switch (ReadInt("Välj: ", 0, 7))
            {
                case 1: ParkVehicle(); break;
                case 2: RetrieveVehicle(); break;
                case 3: ListVehicles(); break;
                case 4: FindVehicle(); break;
                case 5: SearchVehicles(); break;
                case 6: ShowStatistics(); break;
                case 7: ShowCapacity(); break;
                case 0: running = false; break;
            }
        }

        Console.WriteLine("Tack för att du använde Garage 3.0!");
    }

    private static void ShowMainMenu()
    {
        Console.WriteLine();
        Console.WriteLine("----- HUVUDMENY -----");
        Console.WriteLine("1. Parkera fordon");
        Console.WriteLine("2. Hämta ut fordon");
        Console.WriteLine("3. Lista alla fordon");
        Console.WriteLine("4. Hitta fordon via registreringsnummer");
        Console.WriteLine("5. Sök/filtrera fordon");
        Console.WriteLine("6. Visa statistik");
        Console.WriteLine("7. Visa garagekapacitet");
        Console.WriteLine("0. Avsluta");
    }

    private void ParkVehicle()
    {
        Console.WriteLine("\nFordonstyp: 1. Bil  2. Motorcykel  3. Buss  4. Båt  5. Flygplan");
        int type = ReadInt("Välj fordonstyp: ", 1, 5);
        string reg = ReadText("Registreringsnummer: ");
        string color = ReadText("Färg: ");
        int wheels = ReadInt("Antal hjul: ", 0, 100);

        try
        {
            Vehicle vehicle = type switch
            {
                1 => new Car(reg, color, wheels, ReadText("Bränsletyp (t.ex. bensin/diesel/el): ")),
                2 => new Motorcycle(reg, color, wheels, ReadInt("Cylindervolym (cc): ", 1, 100000)),
                3 => new Bus(reg, color, wheels, ReadInt("Antal sittplatser: ", 1, 1000)),
                4 => new Boat(reg, color, wheels, ReadDouble("Längd i meter: ", 0.01, 10000)),
                5 => new Airplane(reg, color, wheels, ReadInt("Antal motorer: ", 1, 20)),
                _ => throw new InvalidOperationException("Okänd fordonstyp.")
            };

            bool success = Handler.Park(vehicle, out string message);
            PrintResult(success, message);
        }
        catch (ArgumentException ex)
        {
            PrintResult(false, ex.Message);
        }
    }

    private void RetrieveVehicle()
    {
        string reg = ReadText("Ange registreringsnummer: ");
        PrintResult(Handler.Retrieve(reg, out string message), message);
    }

    private void ListVehicles()
    {
        IReadOnlyList<Vehicle> vehicles = Handler.GetAll();
        if (vehicles.Count == 0)
        {
            Console.WriteLine("Garaget är tomt.");
            return;
        }

        Console.WriteLine("\nParkerade fordon:");
        foreach (Vehicle vehicle in vehicles)
            Console.WriteLine($"- {vehicle}");
    }

    private void FindVehicle()
    {
        string reg = ReadText("Registreringsnummer: ");
        Vehicle? vehicle = Handler.Find(reg);
        if (vehicle is null)
            PrintResult(false, "Fordonet hittades inte.");
        else
            Console.WriteLine(vehicle);
    }

    private void SearchVehicles()
    {
        Console.WriteLine("Lämna ett filter tomt om det inte ska användas.");
        string? color = ReadOptionalText("Färg: ");
        int? wheels = ReadOptionalInt("Antal hjul: ", 0, 100);
        string? type = ReadOptionalText("Fordonstyp (Car, Bus, Boat, Motorcycle, Airplane): ");

        IReadOnlyList<Vehicle> matches = Handler.Search(color, wheels, type);
        Console.WriteLine($"Hittade {matches.Count} fordon:");
        foreach (Vehicle vehicle in matches)
            Console.WriteLine($"- {vehicle}");
    }

    private void ShowStatistics()
    {
        IReadOnlyDictionary<string, int> statistics = Handler.GetStatistics();
        if (statistics.Count == 0)
        {
            Console.WriteLine("Garaget är tomt.");
            return;
        }

        Console.WriteLine("Fordon per typ:");
        foreach (var item in statistics)
            Console.WriteLine($"{item.Key}: {item.Value}");
    }

    private void ShowCapacity() =>
        Console.WriteLine($"Platser använda: {Handler.Count}/{Handler.Capacity}");

    private GarageHandler Handler => (GarageHandler)_handler!;

    private static IEnumerable<Vehicle> CreateSampleVehicles()
    {
        // Exempelfordon läggs in automatiskt om kapaciteten räcker.
        return new Vehicle[]
        {
            new Car("ABC123", "Svart", 4, "Bensin"),
            new Motorcycle("MOT456", "Röd", 2, 650),
            new Bus("BUS789", "Blå", 6, 50),
            new Boat("BOA321", "Vit", 0, 7.5),
            new Airplane("AIR654", "Vit", 6, 2)
        };
    }

    private static string ReadText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();

            Console.WriteLine("Indata får inte vara tom. Försök igen.");
        }
    }

    private static string? ReadOptionalText(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine()?.Trim();
    }

    private static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                return value;

            Console.WriteLine($"Ange ett heltal mellan {min} och {max}.");
        }
    }

    private static int? ReadOptionalInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return null;

            if (int.TryParse(input, out int value) && value >= min && value <= max)
                return value;

            Console.WriteLine($"Ange ett heltal mellan {min} och {max}, eller lämna tomt.");
        }
    }

    private static double ReadDouble(string prompt, double min, double max)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? "").Trim().Replace(',', '.');
            if (double.TryParse(input, System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.InvariantCulture, out double value)
                && value >= min && value <= max)
                return value;

            Console.WriteLine($"Ange ett tal mellan {min} och {max}.");
        }
    }

    private static void PrintResult(bool success, string message)
    {
        Console.WriteLine($"{(success ? "OK" : "FEL")}: {message}");
    }
}
