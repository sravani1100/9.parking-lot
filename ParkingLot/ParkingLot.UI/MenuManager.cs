using ParkingLot.Common.Enums;

namespace ParkingLot.UI;

public class MenuManager
{
    private readonly ParkingSpaceManager _parkingSpaceManager;

    public MenuManager(ParkingSpaceManager parkingSpaceManager)
    {
        _parkingSpaceManager = parkingSpaceManager;
    }

    public void Run()
    {
        _parkingSpaceManager.ValidateAdminPassword();
        while (true)
        {
            Menu.ShowMainMenu();

            Console.Write("\nPlease Enter Your Choice: ");
            string? input = Console.ReadLine();

            MainMenuOptions menuOption = Enum.TryParse(input, out MainMenuOptions result) ? result : default;

            switch (menuOption)
            {
                case MainMenuOptions.ParkVehicle:
                    {
                        _parkingSpaceManager.ParkVehicle();
                        break;
                    }

                case MainMenuOptions.UnParkVehicle:
                    {
                        _parkingSpaceManager.UnParkVehicle();
                        break;
                    }

                case MainMenuOptions.TotalSlots:
                    {
                        _parkingSpaceManager.DisplayTotalSlots();
                        break;
                    }

                case MainMenuOptions.OccupiedSlots:
                    {
                        _parkingSpaceManager.DisplayOccupiedSlots();
                        break;
                    }

                case MainMenuOptions.AvailableSlots:
                    {
                        _parkingSpaceManager.DisplayAvailableSlots();
                        break;
                    }

                case MainMenuOptions.ActiveTickets:
                    {
                        _parkingSpaceManager.ShowActiveTickets();
                        break;
                    }

                case MainMenuOptions.TotalPrice:
                    {
                        _parkingSpaceManager.DisplayTotalRevenue();
                        break;
                    }

                case MainMenuOptions.Exit:
                    {
                        Console.WriteLine("\n******************* Thank you *******************");
                        return;
                    }
                default:
                    {
                        Console.WriteLine("\nInvalid Choice.");
                        break;
                    }
            }
        }
    }
}
