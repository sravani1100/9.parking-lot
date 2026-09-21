
using P = ParkingLot.Common.Entities;

namespace ParkingLot.Common.Session;

public static class Session
{
    public static P.ParkingLot? CurrentParkingLot { get; set; }
}
