namespace ParkingLot.UI;

public class Menu
{
    public static void ShowMainMenu()
    {
        Console.WriteLine("\n============= MENU =============");
        Console.WriteLine("Please Choose From Below: ");
        Console.WriteLine("1.Park Vehicle");
        Console.WriteLine("2.UnPark Vehicle");
        Console.WriteLine("3.Total Slots");
        Console.WriteLine("4.Available Slots");
        Console.WriteLine("5.Occupied Slots");
        Console.WriteLine("6.Active Tickets");
        Console.WriteLine("7.Total Revenue By Date");
        Console.WriteLine("8.Exit");
    }

    public static void ShowVehicleTypeMenu()
    {
        Console.WriteLine("\n============= VEHICLE TYPE MENU =============");
        Console.WriteLine("Please Choose Vehicle Type From Below: ");
        Console.WriteLine("1.Two Wheeler");
        Console.WriteLine("2.Four Wheeler");
        Console.WriteLine("3.Heavy Vehicle");
    }
}
