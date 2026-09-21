using ParkingLot.Common.Entities;

namespace ParkingLot.DAL.Interfaces;

public interface ITicketRepository
{
    Ticket GetById(string ticketNumber);

    List<Ticket> GetAll();

    Ticket Add(Ticket ticket);

    Ticket Update(Ticket ticket);
}
