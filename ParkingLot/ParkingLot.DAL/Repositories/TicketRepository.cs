using ParkingLot.Common.Entities;
using ParkingLot.Common.Session;
using ParkingLot.DAL.Interfaces;
using P = ParkingLot.Common.Entities;

namespace ParkingLot.DAL.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly P.ParkingLot _parkingLot;

    public TicketRepository(P.ParkingLot parkingLot)
    {
        _parkingLot = parkingLot;
    }

    public Ticket GetById(string ticketNumber)
    {
        if (string.IsNullOrWhiteSpace(ticketNumber))
            throw new Exception("Provide valid ticket number.");

        var ticket = _parkingLot.Tickets.FirstOrDefault(t => t.TicketNumber == ticketNumber);

        if (ticket == null)
            throw new Exception("Ticket not found.");

        return ticket;
    }

    public List<Ticket> GetAll()
    {
        return _parkingLot
            .Tickets
            .Where(ticket => ticket.ParkingLotId == Session.CurrentParkingLot?.ParkingLotId)
            .ToList();
    }

    public Ticket Add(Ticket ticket)
    {
        if (ticket == null)
            throw new Exception("Provide valid ticket.");

        _parkingLot.Tickets.Add(ticket);

        return ticket;
    }

    public Ticket Update(Ticket ticket)
    {
        if (ticket == null)
            throw new Exception("Please provide valid ticket");

        Ticket currentTicket = GetById(ticket.TicketNumber);

        if (currentTicket == null)
            throw new Exception("Ticket not found.");

        int index = _parkingLot.Tickets.IndexOf(currentTicket);

        _parkingLot.Tickets[index] = ticket;

        return ticket;
    }
}

