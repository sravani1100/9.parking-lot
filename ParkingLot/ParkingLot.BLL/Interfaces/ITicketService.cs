using ParkingLot.Common.Entities;
using ParkingLot.Common.Enums;

namespace ParkingLot.BLL.Interfaces;

public interface ITicketService
{
    Ticket GetTicket(string ticketNumber);

    List<Ticket> GetActiveTickets();

    decimal? GetTotalAmountByType(DateTime inTime, VehicleType vehicleType);

    decimal? GetTotalRevenue(DateTime inTime);

    decimal CalculateCost(Ticket ticket, DateTime outtime);

    Ticket GenerateTicket(string ticketNumber, VehicleType vehicleType, string slotNumber);

    Ticket UpdateTicket(Ticket ticket);
}
