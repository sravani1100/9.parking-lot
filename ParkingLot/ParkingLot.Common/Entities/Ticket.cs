using ParkingLot.Common.Enums;

namespace ParkingLot.Common.Entities;

public class Ticket
{
    public string TicketNumber { get; set; } = string.Empty;

    public int ParkingLotId { get; set; }

    public string VehicleNumber { get; set; } = string.Empty;

    public string SlotNumber { get; set; } = string.Empty;

    public DateTime InTime { get; set; }

    public DateTime? OutTime { get; set; }

    public decimal? TotalAmount { get; set; }

    public Ticket(string ticketNumber,
                string vehicleNumber,
                string slotNumber)
    {
        TicketNumber = ticketNumber;
        VehicleNumber = vehicleNumber;
        SlotNumber = slotNumber;
        InTime = DateTime.UtcNow;
    }
}
