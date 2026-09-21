namespace ParkingLot.Common.Entities;

public class ParkingLot
{

    public int ParkingLotId { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<Slot> Slots { get; set; } = new List<Slot>();

    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
}

