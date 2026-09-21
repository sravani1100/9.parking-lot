using ParkingLot.Common.Entities;
using ParkingLot.Common.Enums;

namespace ParkingLot.DAL.Interfaces;

public interface ISlotRepository
{
    List<Slot> GetAll();

    Slot GetById(string slotNumber);

    Slot? GetSloyByType(VehicleType? type);

    Slot Add(Slot slot);

    Slot Update(Slot slot);

    bool Delete(Slot slot);
}