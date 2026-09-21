using ConsoleTables;
using ParkingLot.BLL.Interfaces;
using ParkingLot.Common.Entities;
using P = ParkingLot.Common.Entities; 
using ParkingLot.Common.Enums;
using ParkingLot.Common.Session;
using ParkingLot.UI.Enums;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ParkingLot.UI;

public class ParkingSpaceManager
{
    private readonly IParkingLotService _parkingLotService;
    private readonly ISlotService _slotService;
    private readonly ITicketService _ticketService;

    private string vehicleNumberPattern = @"^[A-Za-z]{2}\d{2}[A-Za-z]{2}\d{4}$";
    private string ticketNumberPattern = @"^PL[0-9]{4}$";

    public ParkingSpaceManager(IParkingLotService parkingLotService,
                            ISlotService slotService,
                            ITicketService ticketService)
    {
        _parkingLotService = parkingLotService;
        _slotService = slotService;
        _ticketService = ticketService;
    }

    public void ValidateAdminPassword()
    {
        Console.WriteLine($"************** Welcome To {ConfigurationManager.AppSettings["ParkingLotName"]} *******************");
        while (true)
        {
            Console.Write("\nEnter Password: ");

            string? password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password Should not be Empty. Please Enter Valid Password.");
                continue;
            }

            if (password != ConfigurationManager.AppSettings["AdminPassword"])
            {
                Console.WriteLine("Invalid Password.Please Enter Valid Password.");
                continue;
            }

            string? parkingLotIdValue = ConfigurationManager.AppSettings["ParkingLotId"];

            if (!int.TryParse(parkingLotIdValue, out int parkingLotId))
            {
                throw new Exception("Invalid ParkingLotId.");
            }

            Session.CurrentParkingLot = new P.ParkingLot
            {
                ParkingLotId = parkingLotId,

                Name = ConfigurationManager.AppSettings["ParkingLotName"] ?? string.Empty
            };

            InitializeSlots();
            return;
        }
    }

    public void InitializeSlots()
    {
        Dictionary<VehicleType, int> parkingSlots = new Dictionary<VehicleType, int>();
        try
        {
            int twoWheelerSlots = ReadSlotInput("\nEnter Number Of Two Wheeler Slots: ");
            parkingSlots.Add(VehicleType.TwoWheeler, twoWheelerSlots);

            int fourWheelerSlots = ReadSlotInput("\nEnter Number Of Four Wheeler Slots: ");
            parkingSlots.Add(VehicleType.FourWheeler, fourWheelerSlots);

            int heavyVehicleSlots = ReadSlotInput("\nEnter Number Of Heavy Vehicle Slots: ");
            parkingSlots.Add(VehicleType.HeavyVehicle, heavyVehicleSlots);

            _parkingLotService.InitializeSlots(parkingSlots);

            Console.WriteLine("\n** Slots Initialized Successfully **");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
    }

    public void ParkVehicle()
    {
        Menu.ShowVehicleTypeMenu();

        try
        {
            VehicleType vehicleType = ReadVehicleType();

            Slot? availableSlot = _slotService.GetAvailableSlot(vehicleType);

            if (availableSlot == null)
            {
                Console.WriteLine($"\n** No Slot Available for {vehicleType}. **");
                return;
            }

            string vehicleNumber = ReadInput("\nEnter Vehicle Number (TG01AB1234): ", vehicleNumberPattern, "Vehicle");

            Vehicle vehicle = CreateVehicle(vehicleType, vehicleNumber);

            Ticket ticket = _parkingLotService.ParkVehicle(vehicle, availableSlot);

            Console.WriteLine("\n=====Ticket=======");
            Console.WriteLine($"Ticket Number   : {ticket.TicketNumber}");
            Console.WriteLine($"Vehicle Number  : {ticket.VehicleNumber}");
            Console.WriteLine($"Slot Number     : {ticket.SlotNumber}");
            Console.WriteLine($"In Time         : {ticket.InTime}");

            Console.WriteLine("\n** Vehicle Parked Successfully **");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void UnParkVehicle()
    {
        try
        {
            if (_ticketService.GetActiveTickets().Count == 0)
            {
                Console.WriteLine("\n** No Vehicle Parked to Unpark **");
                return;
            }

            DateTime outTime = DateTime.UtcNow;
            string ticketNumber = ReadInput("\nEnter Ticket Number(PL8888): ", ticketNumberPattern, "Ticket");

            Ticket ticket = _ticketService.GetTicket(ticketNumber);
            
            decimal amount = _ticketService.CalculateCost(ticket, outTime);
            Console.WriteLine($"Amout to pay: {amount} rupees");

            if (ReadConfirmation("Is Bill Paid?"))
            {
                _parkingLotService.UnParkVehicle(ticket, outTime, amount);
                Console.WriteLine("\n** Vehicle Unparked Successfully **");
            }
            else
            {
                Console.WriteLine("\n** Vehicle UnParking Failed **");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }

    public void ShowActiveTickets()
    {
        List<Ticket> activeTickets = _ticketService.GetActiveTickets();

        if (activeTickets.Count == 0)
        {
            Console.WriteLine("\n** No Active Tickets Available **");
            return;
        }

        var table = new ConsoleTable(
            "Ticket Number",
            "Slot Number",
            "Vehicle Number",
            "In Time");

        foreach (var ticket in activeTickets)
        {
            table.AddRow(
                ticket.TicketNumber,
                ticket.SlotNumber,
                ticket.VehicleNumber,
                ticket.InTime);
        }

        table.Write(Format.Alternative);
    }

    public void ShowSlotsTable(List<Slot> slots, string message)
    {
        if (slots.Count == 0)
        {
            Console.WriteLine(message);
            return;
        }

        var table = new ConsoleTable(
            "Slot Number",
            "Slot Type",
            "Vehicle Number");

        foreach (var slot in slots)
        {
            table.AddRow(
                FormatValue(slot.SlotNumber),
                FormatValue(slot.SlotType),
                FormatValue(slot.Vehicle?.VehcleNumber));
        }

        table.Write(Format.Alternative);
    }

    public void DisplayTotalSlots()
    {
        ShowSlotsTable(_slotService.GetAll(), "\n** No Slot Available **");
    }

    public void DisplayOccupiedSlots()
    {
        ShowSlotsTable(_slotService.GetOccupiedSlots(), "\n ** No Slot is Occupied **");
    }

    public void DisplayAvailableSlots()
    {
        Menu.ShowVehicleTypeMenu();

        VehicleType vehicleType = ReadVehicleType();

        ShowSlotsTable(_slotService.GetAvailableSlotsByVehicleType(vehicleType), "\n** No Slot Available **");

    }

    public void DisplayTotalRevenue()
    {
        DateTime date = ReadDate();

        var table = new ConsoleTable(
            "Vehicle Type",
            "Total Revenue");

        table.AddRow("Two Wheeler", _ticketService.GetTotalAmountByType(date, VehicleType.TwoWheeler) ?? 0);
        table.AddRow("Four Wheeler", _ticketService.GetTotalAmountByType(date, VehicleType.FourWheeler) ?? 0);
        table.AddRow("Heavy Vehicle", _ticketService.GetTotalAmountByType(date, VehicleType.HeavyVehicle) ?? 0);

        table.AddRow("Total Price", _ticketService.GetTotalRevenue(date) ?? 0);

        table.Write(Format.Alternative);
    }

    private int ReadSlotInput(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int number))
            {
                Console.WriteLine("Invaid input. Only numbers are allowed.");
                continue;
            }

            if (number <= 0)
            {
                Console.WriteLine("Number Should be Greater Than 0.");
                continue;
            }

            return number;
        }
    }

    private DateTime ReadDate()
    {
        while (true)
        {
            Console.Write("\nEnter Date (MM-dd-yyyy or M-d-yy): ");
            string? input = Console.ReadLine()?.Trim();

            bool isValid = DateTime.TryParseExact(
                input,
                new[] { "MM-dd-yyyy", "M-d-yy" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime date
            );

            if (!isValid)
            {
                Console.WriteLine("Invalid Date. Please enter in MM-dd-yyyy format.");
                continue;
            }

            return date;
        }
    }

    private static string FormatValue(Object? value)
    {
        if (value == null)
            return "NA";

        string? str = value.ToString();

        return string.IsNullOrWhiteSpace(str) ? "NA" : str;
    }

    private VehicleType ReadVehicleType()
    {
        while (true)
        {
            Console.Write("\nPlease Enter Your Choice For Vehicle Type: ");
            string? input = Console.ReadLine();

            if (Enum.TryParse(input, out VehicleType vehicleType) && Enum.IsDefined(typeof(VehicleType), vehicleType))
                return vehicleType;

            Console.WriteLine("Invalid Choice. Please Enter Valid Choice.");
        }
    }

    private string ReadInput(string message, string validPattern, string field)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input Cannot be Empty.");
                continue;
            }

            if (!Regex.IsMatch(input, validPattern, RegexOptions.IgnoreCase))
            {
                Console.WriteLine($"Invalid Format. Please Provide Valid {field} Number.");
                continue;
            }

            if (string.Equals(field, "Vehicle"))
            {
                string stateCode = input.Substring(0, 2);

                if (!Enum.TryParse(stateCode, true, out StateCode code))
                {
                    Console.WriteLine("Invalid State Code.");
                    continue;
                }
            }

            return input;
        }
    }

    private bool ReadConfirmation(string message)
    {
        while (true)
        {
            Console.Write($"\n{message} (Y/N): ");
            string? input = Console.ReadLine()?.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input Cannot be Empty.");
                continue;
            }

            if (input == "Y")
                return true;

            if (input == "N")
                return false;

            Console.WriteLine("Please Enter Y or N.");
        }
    }

    private Vehicle CreateVehicle(VehicleType vehicleType,
                                    string vehicleNumber)
    {
        return vehicleType switch
        {
            VehicleType.TwoWheeler => new TwoWheeler(vehicleNumber),
            VehicleType.FourWheeler => new FourWheeler(vehicleNumber),
            VehicleType.HeavyVehicle => new HeavyVehicle(vehicleNumber),
            _ => throw new Exception("Invalid Vehicle")
        };
    }
}



