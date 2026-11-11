namespace Garage3.Models;

/// <summary>
/// Bil med en fordonsspecifik egenskap: bränsletyp.
/// </summary>
public sealed class Car : Vehicle
{
    public string FuelType { get; }

    public Car(string registrationNumber, string color, int numberOfWheels, string fuelType)
        : base(registrationNumber, color, numberOfWheels)
    {
        if (string.IsNullOrWhiteSpace(fuelType))
            throw new ArgumentException("Bränsletyp måste anges.", nameof(fuelType));

        FuelType = fuelType.Trim();
    }

    public override string ToString() => $"{base.ToString()}, bränsle: {FuelType}";
}
