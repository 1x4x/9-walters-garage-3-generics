using Garage3.Models;

namespace Garage3.Interfaces;

/// <summary>
/// Funktionerna som användargränssnittet behöver från hanteringslagret.
/// UI:t behöver därmed inte känna till Garage-klassens implementation.
/// </summary>
public interface IHandler
{
    int Capacity { get; }
    int Count { get; }
    bool Park(Vehicle vehicle, out string message);
    bool Retrieve(string registrationNumber, out string message);
    Vehicle? Find(string registrationNumber);
    IReadOnlyList<Vehicle> GetAll();
    IReadOnlyDictionary<string, int> GetStatistics();
    IReadOnlyList<Vehicle> Search(string? color, int? wheels, string? vehicleType);
}
