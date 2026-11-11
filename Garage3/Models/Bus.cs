namespace Garage3.Models;

/// <summary>
/// Buss med antal sittplatser som extra egenskap.
/// </summary>
public sealed class Bus : Vehicle
{
    public int NumberOfSeats { get; }

    public Bus(string registrationNumber, string color, int numberOfWheels, int numberOfSeats)
        : base(registrationNumber, color, numberOfWheels)
    {
        if (numberOfSeats <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfSeats), "Antal sittplatser måste vara större än noll.");

        NumberOfSeats = numberOfSeats;
    }

    public override string ToString() => $"{base.ToString()}, sittplatser: {NumberOfSeats}";
}
