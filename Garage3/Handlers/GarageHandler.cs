using Garage3.Interfaces;
using Garage3.Models;

namespace Garage3.Handlers;

/// <summary>
/// Mellanlager mellan UI och Garage. Här samlas användningsfall och meddelanden.
/// </summary>
public sealed class GarageHandler : IHandler
{
    private readonly IGarage<Vehicle> _garage;

    public int Capacity => _garage.Capacity;
    public int Count => _garage.Count;

    public GarageHandler(int capacity, IEnumerable<Vehicle>? initialVehicles = null)
    {
        _garage = new Garage<Vehicle>(capacity, initialVehicles);
    }

    public bool Park(Vehicle vehicle, out string message)
    {
        if (_garage.Add(vehicle))
        {
            message = $"{vehicle.RegistrationNumber} parkerades.";
            return true;
        }

        message = _garage.Find(vehicle.RegistrationNumber) is not null
            ? "Det finns redan ett fordon med det registreringsnumret."
            : "Garaget är fullt.";
        return false;
    }

    public bool Retrieve(string registrationNumber, out string message)
    {
        Vehicle? vehicle = _garage.Find(registrationNumber);
        if (vehicle is null)
        {
            message = "Inget fordon med det registreringsnumret hittades.";
            return false;
        }

        _garage.Remove(registrationNumber);
        message = $"{vehicle.RegistrationNumber} hämtades ut.";
        return true;
    }

    public Vehicle? Find(string registrationNumber) => _garage.Find(registrationNumber);

    public IReadOnlyList<Vehicle> GetAll() => _garage.ToList();

    public IReadOnlyDictionary<string, int> GetStatistics() =>
        _garage.GroupBy(vehicle => vehicle.VehicleType)
            .OrderBy(group => group.Key)
            .ToDictionary(group => group.Key, group => group.Count());

    /// <summary>
    /// Alla tre filter är valfria. Angivna filter kombineras med AND.
    /// </summary>
    public IReadOnlyList<Vehicle> Search(string? color, int? wheels, string? vehicleType)
    {
        IEnumerable<Vehicle> result = _garage;

        if (!string.IsNullOrWhiteSpace(color))
            result = result.Where(v => v.Color.Equals(color.Trim(), StringComparison.OrdinalIgnoreCase));

        if (wheels.HasValue)
            result = result.Where(v => v.NumberOfWheels == wheels.Value);

        if (!string.IsNullOrWhiteSpace(vehicleType))
            result = result.Where(v => v.VehicleType.Equals(vehicleType.Trim(), StringComparison.OrdinalIgnoreCase));

        return result.ToList();
    }
}
