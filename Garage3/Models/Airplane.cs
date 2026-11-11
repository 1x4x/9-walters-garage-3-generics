namespace Garage3.Models;

/// <summary>
/// Flygplan med antal motorer som extra egenskap.
/// </summary>
public sealed class Airplane : Vehicle
{
    public int NumberOfEngines { get; }

    public Airplane(string registrationNumber, string color, int numberOfWheels, int numberOfEngines)
        : base(registrationNumber, color, numberOfWheels)
    {
        if (numberOfEngines <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfEngines), "Antal motorer måste vara större än noll.");

        NumberOfEngines = numberOfEngines;
    }

    public override string ToString() => $"{base.ToString()}, motorer: {NumberOfEngines}";
}
