using ParkingLot.Common.Enums;
using ParkingLot.UI.Enums;

namespace ParkingLot.Common.Entities;

public class TwoWheeler : Vehicle
{

    public override VehicleType VehicleType => VehicleType.TwoWheeler;
    public TwoWheeler(string vehicleNumber)
    {
        VehcleNumber = vehicleNumber;
    }

    public override decimal GetHourlyCharge()
    {
        return (decimal)HourlyCharge.TwoWheelerCharge;
    }
}

