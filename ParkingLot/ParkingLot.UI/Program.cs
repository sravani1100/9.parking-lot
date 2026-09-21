using ParkingLot.DAL.Repositories;
using ParkingLot.UI;
using ParkingLot.BLL.Interfaces;
using ParkingLot.DAL.Interfaces;
using ParkingLot.BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using P = ParkingLot.Common.Entities;



var services = new ServiceCollection();

//Repositorie
services.AddSingleton<ISlotRepository, SlotRepository>();
services.AddSingleton<ITicketRepository, TicketRepository>();

//Service
services.AddTransient<ISlotService, SlotService>();
services.AddTransient<ITicketService, TicketService>();
services.AddTransient<IParkingLotService, ParkingLotService>();
services.AddTransient<IVehicleService, VehicleService>();

//views
services.AddSingleton<P.ParkingLot>();
services.AddSingleton<MenuManager>();
services.AddSingleton<ParkingSpaceManager>();

//Build Container
var serviceProvider = services.BuildServiceProvider();

//MainMenu
MenuManager mainMenu = serviceProvider.GetRequiredService<MenuManager>();

mainMenu.Run();
