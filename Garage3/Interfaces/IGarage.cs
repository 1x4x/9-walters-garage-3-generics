using Garage3.Models;

namespace Garage3.Interfaces;

/// <summary>
/// Det publika kontraktet som ett garage erbjuder.
/// T begränsas till Vehicle så att garaget bara kan lagra fordon.
/// </summary>
public interface IGarage<T> : IEnumerable<T> where T : Vehicle
{
    int Capacity { get; }
    int Count { get; }
    bool Add(T vehicle);
    bool Remove(string registrationNumber);
    T? Find(string registrationNumber);
    IEnumerable<T> Search(Func<T, bool> predicate);
}
