namespace Garage3.Models;

/// <summary>
/// Motorcykel med cylindervolym som extra egenskap.
/// </summary>
public sealed class Motorcycle : Vehicle
{
    public int CylinderVolume { get; }

    public Motorcycle(string registrationNumber, string color, int numberOfWheels, int cylinderVolume)
        : base(registrationNumber, color, numberOfWheels)
    {
        if (cylinderVolume <= 0)
            throw new ArgumentOutOfRangeException(nameof(cylinderVolume), "Cylindervolym måste vara större än noll.");

        CylinderVolume = cylinderVolume;
    }

    public override string ToString() => $"{base.ToString()}, cylindervolym: {CylinderVolume} cc";
}
