using ParkingLot.Common.Entities;
using ParkingLot.Common.Enums;

namespace ParkingLot.BLL.Interfaces;

public interface IParkingLotService
{
    void InitializeSlots(Dictionary<VehicleType, int> parkingSlots);

    Ticket ParkVehicle(Vehicle vehicle, Slot? availableSlot);

    void UnParkVehicle(Ticket ticket, DateTime? outTime, decimal amount);
}


