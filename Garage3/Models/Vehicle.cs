using Garage3.Interfaces;

namespace Garage3.Models;

/// <summary>
/// Basklass för alla fordon. Gemensamma egenskaper samlas här.
/// Abstract betyder att man inte kan skapa ett generiskt Vehicle-objekt direkt.
/// </summary>
public abstract class Vehicle : IVehicle
{
    // Registreringsnumret normaliseras till versaler för konsekvent visning.
    public string RegistrationNumber { get; }
    public string Color { get; }
    public int NumberOfWheels { get; }
    public string VehicleType => GetType().Name;

    protected Vehicle(string registrationNumber, string color, int numberOfWheels)
    {
        if (string.IsNullOrWhiteSpace(registrationNumber))
            throw new ArgumentException("Registreringsnummer får inte vara tomt.", nameof(registrationNumber));

        if (string.IsNullOrWhiteSpace(color))
            throw new ArgumentException("Färg får inte vara tom.", nameof(color));

        if (numberOfWheels < 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfWheels), "Antal hjul kan inte vara negativt.");

        RegistrationNumber = registrationNumber.Trim().ToUpperInvariant();
        Color = color.Trim();
        NumberOfWheels = numberOfWheels;
    }

    // Varje subklass kan lägga till sina egna egenskaper genom override.
    public override string ToString() =>
        $"{VehicleType}: {RegistrationNumber}, färg: {Color}, hjul: {NumberOfWheels}";
}
