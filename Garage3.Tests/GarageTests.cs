using Garage3.Models;
using Xunit;

namespace Garage3.Tests;

/// <summary>
/// Enhetstester för de publika metoderna i Garage<T>.
/// Testerna följer Arrange–Act–Assert:
/// Arrange = skapa testdata, Act = anropa metoden, Assert = kontrollera resultatet.
/// </summary>
public class GarageTests
{
    [Fact]
    public void Constructor_ZeroCapacity_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        int capacity = 0;

        // Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Garage<Vehicle>(capacity));
    }

    [Fact]
    public void Constructor_NegativeCapacity_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        int capacity = -1;

        // Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Garage<Vehicle>(capacity));
    }

    [Fact]
    public void Constructor_WhenInitialVehiclesFit_PopulatesGarage()
    {
        // Arrange
        Vehicle[] initial =
        {
            new Car("AAA111", "Svart", 4, "Bensin"),
            new Boat("BBB222", "Vit", 0, 6.2)
        };

        // Act
        var garage = new Garage<Vehicle>(3, initial);

        // Assert
        Assert.Equal(2, garage.Count);
        Assert.Equal(3, garage.Capacity);
    }

    [Fact]
    public void Constructor_WhenInitialVehiclesExceedCapacity_ThrowsArgumentException()
    {
        // Arrange
        Vehicle[] initial =
        {
            new Car("AAA111", "Svart", 4, "Bensin"),
            new Bus("BBB222", "Blå", 6, 40)
        };

        // Act + Assert
        Assert.Throws<ArgumentException>(() => new Garage<Vehicle>(1, initial));
    }

    [Fact]
    public void Constructor_WhenInitialVehiclesContainDuplicateRegistration_ThrowsArgumentException()
    {
        // Arrange
        Vehicle[] initial =
        {
            new Car("ABC123", "Svart", 4, "Bensin"),
            new Motorcycle("abc123", "Röd", 2, 500)
        };

        // Act + Assert
        Assert.Throws<ArgumentException>(() => new Garage<Vehicle>(3, initial));
    }

    [Fact]
    public void Add_WhenSpaceAvailable_AddsVehicle()
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);
        Vehicle car = new Car("ABC123", "Svart", 4, "Bensin");

        // Act
        bool result = garage.Add(car);

        // Assert
        Assert.True(result);
        Assert.Equal(1, garage.Count);
        Assert.Same(car, garage.Find("ABC123"));
    }

    [Fact]
    public void Add_WhenGarageIsFull_ReturnsFalse()
    {
        // Arrange
        var garage = new Garage<Vehicle>(1);
        garage.Add(new Car("ABC123", "Svart", 4, "Bensin"));

        // Act
        bool result = garage.Add(new Bus("BUS123", "Blå", 6, 40));

        // Assert
        Assert.False(result);
        Assert.Equal(1, garage.Count);
    }

    [Fact]
    public void Add_WhenRegistrationNumberAlreadyExists_IgnoresLetterCaseAndReturnsFalse()
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);
        garage.Add(new Car("ABC123", "Svart", 4, "Bensin"));

        // Act
        bool result = garage.Add(new Motorcycle("abc123", "Röd", 2, 500));

        // Assert
        Assert.False(result);
        Assert.Equal(1, garage.Count);
    }

    [Fact]
    public void Add_WhenVehicleIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);

        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => garage.Add(null!));
    }

    [Fact]
    public void Find_WhenRegistrationNumberHasDifferentCase_ReturnsVehicle()
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);
        Vehicle expected = new Car("ABC123", "Svart", 4, "Bensin");
        garage.Add(expected);

        // Act
        Vehicle? actual = garage.Find("aBc123");

        // Assert
        Assert.Same(expected, actual);
    }

    [Fact]
    public void Find_WhenVehicleDoesNotExist_ReturnsNull()
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);

        // Act
        Vehicle? result = garage.Find("MISSING");

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Find_WhenRegistrationNumberIsEmptyOrWhitespace_ReturnsNull(string registrationNumber)
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);
        garage.Add(new Car("ABC123", "Svart", 4, "Bensin"));

        // Act
        Vehicle? result = garage.Find(registrationNumber);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Remove_WhenVehicleExists_RemovesVehicle()
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);
        garage.Add(new Car("ABC123", "Svart", 4, "Bensin"));

        // Act
        bool result = garage.Remove("abc123");

        // Assert
        Assert.True(result);
        Assert.Equal(0, garage.Count);
        Assert.Null(garage.Find("ABC123"));
    }

    [Fact]
    public void Remove_WhenVehicleDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);

        // Act
        bool result = garage.Remove("MISSING");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Remove_WhenRegistrationNumberIsEmpty_ReturnsFalse()
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);

        // Act
        bool result = garage.Remove("");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Search_WhenPredicateMatches_ReturnsOnlyMatchingVehicles()
    {
        // Arrange
        var garage = new Garage<Vehicle>(3);
        garage.Add(new Car("AAA111", "Svart", 4, "Bensin"));
        garage.Add(new Motorcycle("BBB222", "Röd", 2, 500));
        garage.Add(new Bus("CCC333", "Svart", 6, 40));

        // Act
        List<Vehicle> result = garage.Search(v => v.Color == "Svart").ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, vehicle => Assert.Equal("Svart", vehicle.Color));
    }

    [Fact]
    public void Search_WhenNoVehicleMatches_ReturnsEmptyCollection()
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);
        garage.Add(new Car("AAA111", "Svart", 4, "Bensin"));

        // Act
        List<Vehicle> result = garage.Search(v => v.Color == "Rosa").ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Search_WhenPredicateIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);

        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => garage.Search(null!));
    }

    [Fact]
    public void Foreach_WhenGarageContainsVehicles_IteratesOverAllVehicles()
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);
        garage.Add(new Car("AAA111", "Svart", 4, "Bensin"));
        garage.Add(new Motorcycle("BBB222", "Röd", 2, 500));

        // Act
        var result = new List<Vehicle>();
        foreach (Vehicle vehicle in garage)
            result.Add(vehicle);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Foreach_WhenGarageIsEmpty_IteratesZeroTimes()
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);
        int iterations = 0;

        // Act
        foreach (Vehicle vehicle in garage)
            iterations++;

        // Assert
        Assert.Equal(0, iterations);
    }
}
