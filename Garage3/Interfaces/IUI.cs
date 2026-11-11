namespace Garage3.Interfaces;

/// <summary>
/// Abstraktion av användargränssnittet.
/// Det gör det möjligt att byta konsolgränssnitt mot exempelvis ett grafiskt UI.
/// </summary>
public interface IUI
{
    void Run();
}
