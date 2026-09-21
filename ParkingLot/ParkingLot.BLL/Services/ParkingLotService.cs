using ParkingLot.BLL.Interfaces;
using ParkingLot.Common.Entities;
using ParkingLot.Common.Enums;
using ParkingLot.DAL.Interfaces;

namespace ParkingLot.BLL.Services;

public class ParkingLotService : IParkingLotService
{

    private readonly ISlotRepository _slotRepository;
    private readonly ITicketService _ticketService;
    private readonly ISlotService _slotService;
    private readonly IVehicleService _vehicleService;

    public ParkingLotService(ISlotRepository slotRepository,
                             ITicketService ticketService,
                             ISlotService slotService,
                             IVehicleService vehicleService)
    {
        _slotRepository = slotRepository;
        _ticketService = ticketService;
        _slotService = slotService;
        _vehicleService = vehicleService;
    }

    public void InitializeSlots(Dictionary<VehicleType, int> parkingSlots)
    {
        int slotNumber = 1;

        if (parkingSlots.Count == 0)
            throw new Exception("Please provide the count before continuing.");

        foreach (var slot in parkingSlots)
        {
            for (int j = 1; j <= slot.Value; j++)
            {
                _slotRepository.Add(new Slot(slotNumber++.ToString(), slot.Key));
            }
        }
    }

    public Ticket ParkVehicle(Vehicle vehicle, Slot? availableSlot)
    {
        if (vehicle == null)
            throw new Exception("Please provide valid vehicle details.");

        Slot? currentVehicle = _slotService
                    .GetOccupiedSlots()
                    .FirstOrDefault(s => s.Vehicle?.VehcleNumber == vehicle.VehcleNumber);

        if (currentVehicle != null)
            throw new Exception("Vehicle Already exists.");

        if (availableSlot == null)
        {
            throw new Exception($"\nNo Slot available for {vehicle.VehicleType}");
        }

        _vehicleService.Park();

        availableSlot.Vehicle = vehicle;

        _slotRepository.Update(availableSlot);

        return _ticketService.GenerateTicket(vehicle.VehcleNumber, vehicle.VehicleType, availableSlot.SlotNumber);
    }

    public void UnParkVehicle(Ticket ticket, DateTime? outTime, decimal amount)
    {
        if (ticket.OutTime != null)
            throw new Exception("No Vehicle Present In The Slot To Unpark.");

        Slot slot = _slotService.GetSlot(ticket.SlotNumber);

        if (slot == null)
            throw new Exception("Slot Not Found.");

        _vehicleService.UnPark();

        slot.Vehicle = null;

        _slotService.UpdateSlot(slot);

        ticket.OutTime = outTime;
        ticket.TotalAmount = amount;

        _ticketService.UpdateTicket(ticket);
    }
}

