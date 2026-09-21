using ParkingLot.Common.Enums;
using ParkingLot.UI.Enums;

namespace ParkingLot.Common.Entities;

public class FourWheeler : Vehicle
{

    public override VehicleType VehicleType => VehicleType.FourWheeler;

    public FourWheeler(string vehicleNumber)
    {
        VehcleNumber = vehicleNumber;
    }

    public override decimal GetHourlyCharge()
    {
        return (decimal)HourlyCharge.FourWheelerCharge;
    }
}

