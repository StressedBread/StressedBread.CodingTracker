using CodingTracker.StressedBread;
using CodingTracker.StressedBread.Controllers;

UserInterface ui = new();
CodingController codingController = new();

codingController.CreateTableOnStart();
ui.MainMenu();