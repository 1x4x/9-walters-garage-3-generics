using System.Collections;
using Garage3.Interfaces;

namespace Garage3.Models;

/// <summary>
/// Generisk samling med fast kapacitet. Garage<Vehicle> kan innehålla alla
/// Vehicle-subklasser, medan Garage<Car> bara kan innehålla bilar.
/// IEnumerable<T> gör att klassen kan användas med foreach.
/// </summary>
public sealed class Garage<T> : IGarage<T> where T : Vehicle
{
    private readonly List<T> _vehicles = new();

    public int Capacity { get; }
    public int Count => _vehicles.Count;

    public Garage(int capacity, IEnumerable<T>? initialVehicles = null)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Garagekapaciteten måste vara större än noll.");

        Capacity = capacity;

        // Startfordon går genom samma validering som fordon som parkeras senare.
        if (initialVehicles is not null)
        {
            foreach (T vehicle in initialVehicles)
            {
                if (!Add(vehicle))
                    throw new ArgumentException(
                        "Startfordonen innehåller dubbletter eller överskrider garagekapaciteten.",
                        nameof(initialVehicles));
            }
        }
    }

    /// <summary>
    /// Parkerar ett fordon om det finns plats och numret inte redan används.
    /// </summary>
    public bool Add(T vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        if (Count >= Capacity || Find(vehicle.RegistrationNumber) is not null)
            return false;

        _vehicles.Add(vehicle);
        return true;
    }

    /// <summary>
    /// Hämtar ut fordonet med angivet registreringsnummer.
    /// Jämförelsen är skiftlägesokänslig.
    /// </summary>
    public bool Remove(string registrationNumber)
    {
        T? vehicle = Find(registrationNumber);
        return vehicle is not null && _vehicles.Remove(vehicle);
    }

    public T? Find(string registrationNumber)
    {
        if (string.IsNullOrWhiteSpace(registrationNumber))
            return null;

        return _vehicles.FirstOrDefault(vehicle =>
            string.Equals(vehicle.RegistrationNumber, registrationNumber.Trim(),
                StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Filtrerar fordon med valfri predikatfunktion, exempelvis LINQ-villkor.
    /// </summary>
    public IEnumerable<T> Search(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return _vehicles.Where(predicate).ToList();
    }

    public IEnumerator<T> GetEnumerator() => _vehicles.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
