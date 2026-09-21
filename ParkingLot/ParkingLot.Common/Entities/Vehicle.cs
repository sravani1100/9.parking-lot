using ParkingLot.Common.Enums;

namespace ParkingLot.Common.Entities;

public abstract class Vehicle
{
    public string VehcleNumber { get; set; } = string.Empty;

    public abstract VehicleType VehicleType { get; }

    public abstract decimal GetHourlyCharge();
}

