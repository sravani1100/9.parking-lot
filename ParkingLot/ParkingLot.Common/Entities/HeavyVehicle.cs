using ParkingLot.Common.Enums;
using ParkingLot.UI.Enums;

namespace ParkingLot.Common.Entities;

public class HeavyVehicle : Vehicle
{
    public override VehicleType VehicleType => VehicleType.HeavyVehicle;

    public HeavyVehicle(string vehicleNumber)
    {
        VehcleNumber = vehicleNumber;
    }

    public override decimal GetHourlyCharge()
    {
        return (decimal)HourlyCharge.HeavyVehicleCharge;
    }
}

