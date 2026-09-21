using ParkingLot.BLL.Interfaces;
using ParkingLot.Common.Entities;
using ParkingLot.Common.Enums;
using ParkingLot.DAL.Interfaces;

namespace ParkingLot.BLL.Services;

public class TicketService : ITicketService
{

    private readonly ITicketRepository _ticketRepository;
    private readonly ISlotRepository _slotRepository;

    public TicketService(ITicketRepository ticketRepository,
        ISlotRepository slotRepository)
    {
        _ticketRepository = ticketRepository;
        _slotRepository = slotRepository;
    }

    public Ticket GetTicket(string ticketNumber)
    {
        if (string.IsNullOrWhiteSpace(ticketNumber))
            throw new Exception("Provide valid ticket number.");

        Ticket ticket = _ticketRepository.GetById(ticketNumber);

        if (ticket == null)
            throw new Exception("Ticket not found.");

        return ticket;
    }

    public List<Ticket> GetActiveTickets()
    {
        return _ticketRepository
            .GetAll()
            .Where(t => t.OutTime == null)
            .ToList();
    }

    public decimal? GetTotalAmountByType(DateTime inTime, VehicleType vehicleType)
    {
        List<Slot> slots = _slotRepository
                .GetAll()
                .Where(s => s.SlotType == vehicleType)
                .ToList();

        List<Ticket> tickets = _ticketRepository
            .GetAll();

        List<Ticket> ticketList = new List<Ticket>();

        foreach (var slot in slots)
        {
            Ticket? ticket = tickets
                    .FirstOrDefault(t => t.SlotNumber == slot.SlotNumber);

            if (ticket != null)
                ticketList.Add(ticket);
        }

        return ticketList
            .Where(t => DateOnly.FromDateTime(t.InTime) == DateOnly.FromDateTime(inTime))
            .Sum(t => t.TotalAmount);
    }

    public decimal? GetTotalRevenue(DateTime inTime)
    {

        return _ticketRepository
            .GetAll()
            .Where(t => DateOnly.FromDateTime(t.InTime) == DateOnly.FromDateTime(inTime)
                        && t.OutTime != null)
            .Sum(t => t.TotalAmount);
    }

    public Ticket GenerateTicket(string vehicleNumber, VehicleType vehicleType, string slotNumber)
    {
        if (string.IsNullOrWhiteSpace(vehicleNumber) || string.IsNullOrWhiteSpace(slotNumber))
            throw new Exception("Please provide valid ticket details.");

        Ticket ticket = new Ticket(GenerateTicketNumber(), vehicleNumber, slotNumber);
        _ticketRepository.Add(ticket);

        return ticket;
    }

    public Ticket UpdateTicket(Ticket ticket)
    {
        if (ticket == null)
            throw new Exception("Please Provide valid ticket to be updated.");

        _ticketRepository.Update(ticket);

        return ticket;
    }

    public decimal CalculateCost(Ticket ticket, DateTime outTime)
    {
        Slot slot = _slotRepository.GetById(ticket.SlotNumber);
      
        int totalMinutes = (int)(outTime - ticket.InTime).TotalMinutes;

        int hours = totalMinutes / 60;

        if (totalMinutes % 60 != 0)
            hours++;

        return hours * slot.Vehicle.GetHourlyCharge();
    
    }

    private string GenerateTicketNumber()
    {
        List<Ticket> tickets = _ticketRepository.GetAll();

        while (true)
        {
            var guid = Guid.NewGuid();
            int hash = Math.Abs(guid.GetHashCode());
            int number = hash % 10000;

            string newTicketNumber = $"PL{number:D4}";
            if (!tickets.Any(t => t.TicketNumber == newTicketNumber))
            {
                return newTicketNumber;
            }
        }
    }
}
