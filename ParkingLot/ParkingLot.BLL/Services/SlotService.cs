using ParkingLot.BLL.Interfaces;
using ParkingLot.Common.Entities;
using ParkingLot.Common.Enums;
using ParkingLot.DAL.Interfaces;

namespace ParkingLot.BLL.Services;

public class SlotService : ISlotService
{
    private readonly ISlotRepository _slotRepository;

    public SlotService(ISlotRepository slotRepository)
    {
        _slotRepository = slotRepository;
    }

    public List<Slot> GetAll()
    {
        return _slotRepository.GetAll();
    }

    public Slot GetSlot(string slotNumber)
    {
        if (string.IsNullOrWhiteSpace(slotNumber))
            throw new Exception("Provide valid slot number.");

        return _slotRepository.GetById(slotNumber);
    }

    public List<Slot> GetOccupiedSlots()
    {
        return _slotRepository
             .GetAll()
             .Where(s => s.Vehicle != null)
             .ToList();
    }

    public Slot? GetAvailableSlot(VehicleType vehicleType)
    {
        return _slotRepository
            .GetAll()
            .FirstOrDefault(s => s.SlotType == vehicleType && s.Vehicle == null);
    }

    public List<Slot> GetAvailableSlotsByVehicleType(VehicleType vehicleType)
    {
        return _slotRepository
            .GetAll()
            .Where(s => s.SlotType == vehicleType && s.Vehicle == null)
            .ToList();
    }

    public Slot AddSlot(Slot slot)
    {
        if (slot == null)
            throw new Exception("Please provide valid slot.");

        _slotRepository.Add(slot);

        return slot;
    }

    public Slot UpdateSlot(Slot slot)
    {
        if (slot == null)
            throw new Exception("Please provide valid slot to be updated.");

        _slotRepository.Update(slot);

        return slot;
    }

    public bool DeleteSlot(Slot slot)
    {
        if (slot == null)
            throw new Exception("Please provide valid slot to be updated.");

        return _slotRepository.Delete(slot);
    }
}

