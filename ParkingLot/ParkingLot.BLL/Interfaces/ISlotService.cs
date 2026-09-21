using ParkingLot.Common.Entities;
using ParkingLot.Common.Enums;

namespace ParkingLot.BLL.Interfaces;

public interface ISlotService
{

    List<Slot> GetAll();

    Slot GetSlot(string slotNumber);

    List<Slot> GetOccupiedSlots();

    Slot? GetAvailableSlot(VehicleType vehicleType);

    List<Slot> GetAvailableSlotsByVehicleType(VehicleType vehicleType);

    Slot AddSlot(Slot slot);

    Slot UpdateSlot(Slot slot);

    bool DeleteSlot(Slot slot);
}

