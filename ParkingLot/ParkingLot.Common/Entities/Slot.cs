using ParkingLot.Common.Enums;
using S = ParkingLot.Common.Session;

namespace ParkingLot.Common.Entities;

public class Slot
{
    public string SlotNumber { get; set; }

    public int ParkingLotId { get; set; }

    public VehicleType SlotType { get; set; }

    public Vehicle? Vehicle { get; set; }

    public Slot(string slotNumber, VehicleType slotType)
    {
        SlotNumber = slotNumber;
        SlotType = slotType;

        if(S.Session.CurrentParkingLot != null)
        {
            ParkingLotId = S.Session.CurrentParkingLot.ParkingLotId;
        }
    }
}
