using ParkingLot.Common.Entities;
using ParkingLot.Common.Enums;
using ParkingLot.Common.Session;
using ParkingLot.DAL.Interfaces;
using P = ParkingLot.Common.Entities; 

namespace ParkingLot.DAL.Repositories;

public class SlotRepository : ISlotRepository
{

    private readonly P.ParkingLot _parkingLot;

    public SlotRepository(P.ParkingLot parkingLot)
    {
        _parkingLot = parkingLot;
    }

    public Slot GetById(string slotNumber)
    {
        if (string.IsNullOrWhiteSpace(slotNumber))
            throw new Exception("Provide valid slot number");

        Slot? slot = _parkingLot.Slots.FirstOrDefault(s => s.SlotNumber == slotNumber);

        if (slot == null)
            throw new Exception("Slot not found.");

        return slot;
    }

    public Slot? GetSloyByType(VehicleType? type)
    {
        if (type == null)
            throw new Exception("Please provide valid Slot type.");

        return GetAll()
            .FirstOrDefault(s => s.SlotType == type);
    }

    public List<Slot> GetAll()
    {
        return _parkingLot
            .Slots
            .Where(slot => slot.ParkingLotId == Session.CurrentParkingLot?.ParkingLotId)
            .ToList();
    }

    public Slot Add(Slot slot)
    {
        if (slot == null)
            throw new Exception("Please provide required slot.");

        _parkingLot.Slots.Add(slot);

        return slot;
    }

    public Slot Update(Slot slot)
    {
        if (slot == null)
            throw new Exception("Please provide valid slot.");

        Slot currentSlot = GetById(slot.SlotNumber);

        if (currentSlot == null)
            throw new Exception("Slot not found.");

        int index = _parkingLot.Slots.IndexOf(currentSlot);

        _parkingLot.Slots[index] = slot;

        return slot;
    }

    public bool Delete(Slot slot)
    {
        Slot currentSlot = GetById(slot.SlotNumber);

        if (currentSlot == null)
            throw new Exception("Slot not found");

        return _parkingLot.Slots.Remove(currentSlot);
    }
}
