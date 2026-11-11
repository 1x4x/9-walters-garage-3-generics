namespace Garage3.Interfaces;

/// <summary>
/// Gemensamt kontrakt för alla fordon.
/// Basklassen Vehicle implementerar detta interface.
/// </summary>
public interface IVehicle
{
    string RegistrationNumber { get; }
    string Color { get; }
    int NumberOfWheels { get; }
    string VehicleType { get; }
}
