namespace Garage3.Models;

/// <summary>
/// Båt med längd i meter som extra egenskap.
/// </summary>
public sealed class Boat : Vehicle
{
    public double Length { get; }

    public Boat(string registrationNumber, string color, int numberOfWheels, double length)
        : base(registrationNumber, color, numberOfWheels)
    {
        if (length <= 0)
            throw new ArgumentOutOfRangeException(nameof(length), "Längden måste vara större än noll.");

        Length = length;
    }

    public override string ToString() => $"{base.ToString()}, längd: {Length:0.##} m";
}
